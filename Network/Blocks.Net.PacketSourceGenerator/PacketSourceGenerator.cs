using System.Text;
using Blocks.Net.LibSourceGeneration.Builders;
using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.Expressions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;
using Blocks.Net.LibSourceGeneration.Statements;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Blocks.Net.PacketSourceGenerator;

[Generator]
public partial class PacketSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }


    // This is a simple remap of types from what we want, to what we need 
    private static readonly Dictionary<string, string> PrimitiveRemap = new()
    {
        { "bool", "Blocks.Net.Packets.Primitives.Boolean" },
        { "byte", "UnsignedByte" },
        { "sbyte", "Blocks.Net.Packets.Primitives.Byte" },
        { "short", "Short" },
        { "ushort", "UnsignedShort" },
        { "int", "Int" },
        { "long", "Long" },
        { "float", "Float" },
        { "double", "Blocks.Net.Packets.Primitives.Double" },
        { "string", "Blocks.Net.Packets.Primitives.String" },
        { "NbtTag", "Blocks.Net.Packets.Primitives.Nbt" },
        { "Guid", "Blocks.Net.Packets.Primitives.Uuid" },
        { "Uuid", "Blocks.Net.Packets.Primitives.Uuid" },
    };

    public void Execute(GeneratorExecutionContext context)
    {
        Dictionary<string, Dictionary<int, string>> serverBoundPackets = [];
        var foundPacketParser = false;

        foreach (var tree in context.Compilation.SyntaxTrees)
        {
            var semanticModel = context.Compilation.GetSemanticModel(tree);
            var ns = tree.GetRoot().DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();
            var usingDecls = tree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
            var usings = usingDecls.Select(x => x.Name!.ToString()).ToArray();
            var namespaceName = ns?.Name.ToString() ?? "";

            var interfaces = tree.GetRoot().DescendantNodes().OfType<InterfaceDeclarationSyntax>();
            var classes = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
            var structs = tree.GetRoot().DescendantNodes().OfType<StructDeclarationSyntax>();

            foreach (var clazz in classes)
            {
                var className = clazz.Identifier.Text;
                var attrs = clazz.DescendantNodes().OfType<AttributeSyntax>().ToArray();
                if ($"{namespaceName}.{className}" == "Blocks.Net.Packets.PacketParser") foundPacketParser = true;
                if (attrs.FirstOrDefault(
                        a => a.DescendantTokens().Any(dt => CheckForPacketAttribute(dt, semanticModel))) is { } attr)
                {
                    var descendentTokens = attr.DescendantTokens().ToArray();
                    var byteNode = descendentTokens.FirstOrDefault(x => x.IsKind(SyntaxKind.NumericLiteralToken));
                    if (byteNode == null) continue;
                    var packetId = (int)(byteNode.Value ?? 0xff);
                    var builder = StartPacketClass(namespaceName, className, packetId, usings, out var @class);

                    var clientBoundToken = descendentTokens.FirstOrDefault(x =>
                        x.IsKind(SyntaxKind.TrueKeyword) || x.IsKind(SyntaxKind.FalseKeyword));
                    var clientBound = clientBoundToken == null || clientBoundToken.IsKind(SyntaxKind.TrueKeyword);
                    var state =
                        descendentTokens.FirstOrDefault(x => x.IsKind(SyntaxKind.StringLiteralToken)) is var syntaxToken
                            ? (string)syntaxToken.Value!
                            : "Play";
                    var read = StartReadFrom(@class, className);
                    var write = StartWrite(@class);
                    List<string> fields = [];
                    foreach (var field in clazz.DescendantNodes().OfType<FieldDeclarationSyntax>())
                    {
                        GeneratePacketFieldImpl(field, semanticModel, read, write, fields);
                    }

                    EndReadFrom(read, fields.ToArray());

                    if (!clientBound)
                    {
                        var dict = serverBoundPackets.TryGetValue(state, out var d)
                            ? d
                            : serverBoundPackets[state] = [];
                        dict[packetId] = $"{namespaceName}.{className}";
                    }

                    context.AddSource($"{namespaceName}.{className}.g.cs", builder.Build());
                }
            }

            foreach (var structure in structs)
            {
                var structName = structure.Identifier.Text;
                var attrs = structure.DescendantNodes().OfType<AttributeSyntax>().ToArray();
                if (attrs.FirstOrDefault(a =>
                        a.DescendantTokens().Any(dt => CheckForSubPacketAttribute(dt, semanticModel))) is { } attr)
                {
                    var builder = StartSubPacketStruct(namespaceName, structName, usings, out var @struct);

                    var read = StartReadFrom(@struct, structName, true);
                    var write = StartWrite(@struct, true);
                    List<string> fields = [];
                    // Now let's go over every field
                    foreach (var field in structure.DescendantNodes().OfType<FieldDeclarationSyntax>())
                    {
                        GeneratePacketFieldImpl(field, semanticModel, read, write, fields);
                    }

                    EndReadFrom(read, fields.ToArray());

                    context.AddSource($"{namespaceName}.{structName}.g.cs", builder.Build());
                }
            }
        }

        if (foundPacketParser)
        {
            var builder = new SourceFileBuilder().WithFileScopedNamespace("Blocks.Net.Packets").AddClass("PacketParser",
                @class =>
                {
                    @class.Static().Partial();
                    foreach (var kvp in serverBoundPackets)
                    {
                        var state = kvp.Key;
                        var delegates = kvp.Value;
                        @class.AddField("Dictionary<int,Func<MemoryStream,IPacket>>", $"{state}ServerBoundPackets",
                            field =>
                            {
                                field.Public().Static();
                                field.Default(new NewObject().InitializeWith(init =>
                                {
                                    foreach (var kvp2 in delegates)
                                    {
                                        init.Add(new CollectionInitializer().Add(
                                            new IntegerLiteral(kvp2.Key, @base: IntegerBase.Hexadecimal),
                                            new GetStatic(kvp2.Value, "ReadFrom")));
                                    }
                                }));
                            });
                    }
                });
            context.AddSource("PacketParser.g.cs", builder.Build());
        }
    }

    private void GeneratePacketFieldImpl(FieldDeclarationSyntax field, SemanticModel semanticModel,
        MethodReference read, MethodReference write, List<string> fields)
    {
        // Now let's check first if the field has a very simple attribute
        var fieldAttrs = field.DescendantNodes().OfType<AttributeSyntax>().ToArray();
        var decl = field.Declaration;
        var targetType = decl.Type.GetText().ToString().Trim();
        var name = decl.Variables.First().Identifier.Text;
        // The simplest type of packet field to handle
        if (fieldAttrs.FirstOrDefault(a =>
                a.DescendantTokens().Any(dt => CheckForPacketFieldAttribute(dt, semanticModel))) is { } fieldAttr)
        {
            var primitiveType = PrimitiveRemap.TryGetValue(targetType, out var primType)
                ? primType
                : targetType;
            AddSimpleFieldRead(read, name, targetType, primitiveType);
            AddSimpleFieldWrite(write, name, targetType, primitiveType);
            fields.Add(name);
        }

        // Almost as simple
        if (fieldAttrs.FirstOrDefault(a =>
                a.DescendantTokens().Any(dt => CheckForPacketEnumAttribute(dt, semanticModel))) is
            { } enumAttr)
        {
            var descendantTokens = enumAttr.DescendantTokens().ToArray();
            var identNode = descendantTokens.LastOrDefault(dt => dt.IsKind(SyntaxKind.IdentifierToken));
            var relatedType = semanticModel.GetTypeInfo(identNode.Parent!).Type!.Name;
            var primitiveType = PrimitiveRemap.TryGetValue(relatedType, out var primType)
                ? primType
                : relatedType;
            AddEnumFieldRead(read, name, targetType, primitiveType);
            AddEnumFieldWrite(write, name, primitiveType);
            fields.Add(name);
        }

        if (fieldAttrs.FirstOrDefault(a =>
                a.DescendantTokens().Any(dt => CheckForPacketArrayFieldAttribute(dt, semanticModel))) is
            { } arrayAttr)
        {
            var descendantTokens = arrayAttr.DescendantTokens().ToArray();
            var stringNode = descendantTokens.LastOrDefault(dt => dt.IsKind(SyntaxKind.StringLiteralToken));
            var stringValue = (string)stringNode.Value!;
            // We want to remove the [] from the target type here, as we know that will be there
            var targetTypeSubType = targetType.Substring(0, targetType.Length - 2);
            var primitiveSubType = PrimitiveRemap.TryGetValue(targetTypeSubType, out var primSubType)
                ? primSubType
                : targetTypeSubType;
            AddArrayFieldRead(read, name, targetTypeSubType, primitiveSubType, stringValue);
            AddArrayFieldWrite(write, name, targetTypeSubType, primitiveSubType, stringValue);
            fields.Add(name);
        }

        if (fieldAttrs.FirstOrDefault(a =>
                a.DescendantTokens().Any(dt => CheckForPacketOptionalFieldAttribute(dt, semanticModel))) is
            { } optAttr)
        {
            var descendantTokens = optAttr.DescendantTokens().ToArray();
            var stringNode = descendantTokens.LastOrDefault(dt => dt.IsKind(SyntaxKind.StringLiteralToken));
            var stringValue = (string)stringNode.Value!;
            var primitiveType = PrimitiveRemap.TryGetValue(targetType, out var primType)
                ? primType
                : targetType;
            AddOptionalFieldRead(read, name, targetType, primitiveType, stringValue);
            AddOptionalFieldWrite(write, name, targetType, primitiveType, stringValue);
            fields.Add(name);
        }
    }

    private bool CheckForPacketAttribute(SyntaxToken token, SemanticModel semanticModel)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        return name == "Packet";
    }

    private bool CheckForSubPacketAttribute(SyntaxToken token, SemanticModel semanticModel)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        return name == "SubPacket";
    }

    private bool CheckForPacketFieldAttribute(SyntaxToken token, SemanticModel semanticModel)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        return name == "PacketField";
    }

    private bool CheckForPacketEnumAttribute(SyntaxToken token, SemanticModel semanticModel)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        return name == "PacketEnum";
    }

    private bool CheckForPacketArrayFieldAttribute(SyntaxToken token, SemanticModel semanticModel)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        return name == "PacketArrayField";
    }

    private bool CheckForPacketOptionalFieldAttribute(SyntaxToken token, SemanticModel semanticModel)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        return name == "PacketOptionalField";
    }
    

    // Let's do this a simple way
    private SourceFileBuilder StartPacketClass(string ns, string className, int id, IEnumerable<string> usings,
        out StructuredTypeReference @class)
    {
        var sfb = new SourceFileBuilder().WithFileScopedNamespace(ns).Using(usings)
            .Using("Blocks.Net.Packets.Primitives").AddClass(className, out @class);
        @class.Partial().AddProperty("int", "PacketId",
            prop => prop.Public().AddGetter(get => get.Return(new IntegerLiteral(id, @base: IntegerBase.Hexadecimal))));
        return sfb;
    }

    private SourceFileBuilder StartSubPacketStruct(string ns, string structName, IEnumerable<string> usings,
        out StructuredTypeReference @struct)
    {
        var sfb = new SourceFileBuilder().WithFileScopedNamespace(ns).Using(usings)
            .Using("Blocks.Net.Packets.Primitives").AddStruct(structName, out @struct);
        @struct.Partial();
        return sfb;
    }
    

    private MethodReference StartReadFrom(StructuredTypeReference ty, string className, bool isSubPacket = false)
    {
        ty.AddMethod(className, "ReadFrom", out var method);
        return method.Public().Static().WithParameters(new ParameterReference(typeof(MemoryStream), "stream"))
            .WithDocumentation(new DocCommentBuilder()
                .WithSummary($"Reads a {className} {(isSubPacket ? "sub" : "")}packet from a memory stream\n")
                .WithParameter("stream",
                    $"The {(isSubPacket ? "sub" : "")}packet stream{(isSubPacket ? "" : ", the length and id must already be consumed")}")
                .Returns($"A {(isSubPacket ? "sub" : "")}packet constructed from the given stream"));
    }
    
    private void AddSimpleFieldRead(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType)
    {
        var call = new TypeCall(primitiveType, "ReadFrom", new Variable("stream"));
        var init = primitiveType == targetType ? (IExpression)call : new CastExpression(targetType, call);
        method.DeclareVariable(fieldName, init);
    }
    
    private void AddEnumFieldRead(MethodReference method, string fieldName, TypeReference enumType,
        TypeReference primitiveType)
    {
        var call = new TypeCall(primitiveType, "ReadFrom", new Variable("stream"));
        var value = new GetField(call, "Value");
        var cast = new CastExpression(enumType, value);
        method.DeclareVariable(fieldName, cast);
    }
    
    private void AddArrayFieldRead(MethodReference method, string fieldName, TypeReference targetSubType,
        TypeReference primitiveSubType, string lengthControl)
    {
        var lengthVarName = $"__{fieldName}_length__";
        var iterVarName = $"__{fieldName}_iter__";
        method.DeclareVariable(typeof(int), lengthVarName, new InjectedExpression(lengthControl));
        var lengthVar = new Variable(lengthVarName);
        var iterVar = new Variable(iterVarName);
        var field = new Variable(fieldName);
        method.DeclareVariable(fieldName, new NewArray(targetSubType, lengthVar));
        var call = new TypeCall(primitiveSubType, "ReadFrom", new Variable("stream"));
        var init = primitiveSubType == targetSubType ? (IExpression)call : new CastExpression(targetSubType, call);
        var assign = new Assignment(new Subscript(field, iterVar), init);
        method.For(new VariableDeclarationStatement(iterVarName, new IntegerLiteral(0)),
            new LessThan(iterVar, lengthVar), new PostIncrement(iterVar),
            loop => loop.Add(assign));
    }

    private void AddOptionalFieldRead(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType, string optionalControl)
    {
        method.DeclareVariable(fieldName, new Default(targetType));
        var call = new TypeCall(primitiveType, "ReadFrom", new Variable("stream"));
        var init = primitiveType == targetType ? (IExpression)call : new CastExpression(targetType, call);
        var assign = new Assignment(fieldName, init);
        method.If(new InjectedExpression(optionalControl), statement => statement.Add(assign));
    }

    private void EndReadFrom(MethodReference method, string[] fieldNames) =>
        method.Return(new NewObject().InitializeWith(init =>
        {
            foreach (var field in fieldNames)
            {
                init.SetField(field, new Variable(field));
            }
        }));
    
    private MethodReference StartWrite(StructuredTypeReference type, bool isSubPacket = false)
    {
        type.AddMethod("void", isSubPacket ? "WriteTo" : "Write", out var method);
        if (!isSubPacket)
        {
            method.WithDocumentation(DocCommentBuilder.InheritDoc());
        }

        return method.Public().WithParameters(new ParameterReference(typeof(MemoryStream), "stream"));
    }

    private void AddSimpleFieldWrite(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType)
    {
        var field = new Variable(fieldName);
        var stream = new Variable("stream");
        method.Add(new Call(targetType == primitiveType ? field : new CastExpression(primitiveType, field), "WriteTo",
            stream));
    }
    private void AddEnumFieldWrite(MethodReference method, string fieldName, TypeReference primitiveType)
    {
        var field = new Variable(fieldName);
        var stream = new Variable("stream");
        method.Add(new Call(new CastExpression(primitiveType, new CastExpression("int", field)), "WriteTo", stream));
    }
    private void AddArrayFieldWrite(MethodReference method, string fieldName, TypeReference targetSubType,
        TypeReference primitiveSubType, string lengthControl)
    {
        var lengthVarName = $"__{fieldName}_length__";
        var iterVarName = $"__{fieldName}_iter__";
        method.DeclareVariable(typeof(int), lengthVarName, new InjectedExpression(lengthControl));
        var lengthVar = new Variable(lengthVarName);
        var iterVar = new Variable(iterVarName);
        var field = new Variable(fieldName);
        var stream = new Variable("stream");
        IExpression baseVar = new Subscript(field, iterVar);
        if (targetSubType != primitiveSubType)
        {
            baseVar = new CastExpression(primitiveSubType, baseVar);
        }

        var call = new Call(baseVar, "WriteTo", stream);
        method.For(new VariableDeclarationStatement(iterVarName, new IntegerLiteral(0)),
            new LessThan(iterVar, lengthVar), new PostIncrement(iterVar),
            loop => loop.Add(call));
    }

    private void AddOptionalFieldWrite(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType, string optionalControl)
    {
        IExpression baseVar = new Variable(fieldName);
        if (targetType != primitiveType)
        {
            baseVar = new CastExpression(primitiveType, baseVar);
        }

        var stream = new Variable("stream");
        var call = new Call(baseVar, "WriteTo", stream);
        method.If(new InjectedExpression(optionalControl), ifStatement => ifStatement.Add(call));
    }
}