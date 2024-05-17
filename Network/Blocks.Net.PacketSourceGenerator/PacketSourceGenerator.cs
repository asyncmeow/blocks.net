using System.Reflection;
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

    private class FieldedEnumInformation
    {
        public string Namespace = "";
        public string Name;
        public string SubType = "VarInt";
        public List<string> Usings = [];
        public Dictionary<string, string> OverloadedFields = [];
    }

    public void Execute(GeneratorExecutionContext context)
    {
        Dictionary<string, Dictionary<int, string>> serverBoundPackets = [];
        Dictionary<string, FieldedEnumInformation> fieldedEnums = [];
        var foundPacketParser = false;

        // Now we need to have a list of structured type references for our source file builders

        // Dictionary<string,List<(string,string)>> 

        foreach (var tree in context.Compilation.SyntaxTrees)
        {
            var semanticModel = context.Compilation.GetSemanticModel(tree);
            var ns = tree.GetRoot().DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();
            var usingDecls = tree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
            var usings = usingDecls.Select(x => x.Name!.ToString()).ToArray();
            var namespaceName = ns?.Name.ToString() ?? "";

            var interfaces = tree.GetRoot().DescendantNodes().OfType<InterfaceDeclarationSyntax>();
            var enums = tree.GetRoot().DescendantNodes().OfType<EnumDeclarationSyntax>();
            var classes = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
            var structs = tree.GetRoot().DescendantNodes().OfType<StructDeclarationSyntax>();

            foreach (var clazz in classes)
            {
                var className = clazz.Identifier.Text;
                var attrs = clazz.DescendantNodes().OfType<AttributeSyntax>().ToArray();
                if ($"{namespaceName}.{className}" == "Blocks.Net.Packets.PacketParser") foundPacketParser = true;
                if (attrs.FirstOrDefault(
                        a => a.DescendantTokens().Any(dt => CheckForAttribute(dt, semanticModel, "Packet"))) is
                    { } attr)
                {
                    GeneratePacketImplementation(context, attr, namespaceName, className, usings, clazz, semanticModel,
                        serverBoundPackets);
                }

                if (attrs.FirstOrDefault(a =>
                        a.DescendantTokens().Any(dt => CheckForAttribute(dt, semanticModel, "EnumField"))) is
                    { } enumFieldAttr)
                {
                    GenerateEnumFieldImplementation(context, enumFieldAttr, semanticModel, className, fieldedEnums,
                        namespaceName, usings, clazz);
                }
            }

            foreach (var structure in structs)
            {
                var structName = structure.Identifier.Text;
                var attrs = structure.DescendantNodes().OfType<AttributeSyntax>().ToArray();
                if (attrs.FirstOrDefault(a =>
                        a.DescendantTokens().Any(dt => CheckForAttribute(dt, semanticModel, "SubPacket"))) is { } attr)
                {
                    GenerateSubPacketImplementation(context, namespaceName, structName, usings, structure,
                        semanticModel);
                }
            }

            foreach (var enumeration in enums)
            {
                var enumName = enumeration.Identifier.Text;

                var attrs = enumeration.DescendantNodes().OfType<AttributeSyntax>().ToArray();
                if (attrs.FirstOrDefault(
                        a => a.DescendantTokens().Any(dt => CheckForAttribute(dt, semanticModel, "FieldedEnum"))) is
                    { } attr)
                {
                    RegisterFieldedEnum(attr, semanticModel, fieldedEnums, enumName, namespaceName, usings);
                }
            }
        }

        if (foundPacketParser)
        {
            GeneratePacketParser(context, serverBoundPackets);
        }

        foreach (var fieldedEnum in fieldedEnums.Values)
        {
            BuildFieldedEnum(context, fieldedEnum);
        }
    }

    private static void BuildFieldedEnum(GeneratorExecutionContext context, FieldedEnumInformation fieldedEnum)
    {
        var sfb = new SourceFileBuilder()
            .WithFileScopedNamespace(fieldedEnum.Namespace)
            .Using(fieldedEnum.Usings)
            .Nullable()
            .AddInterface($"I{fieldedEnum.Name}",
                enumInterface => enumInterface.Public().AddMethod("void", "Write",
                    method => method.WithoutImplementation()
                        .WithParameters(new ParameterReference(typeof(MemoryStream), "stream")).Public()))
            .AddStruct($"{fieldedEnum.Name}Impl",
                enumStruct => enumStruct.Public()
                    .AddField(fieldedEnum.Name, "Type", type => type.Public())
                    .AddField($"I{fieldedEnum.Name}?", "Data", value => value.Public())
                    .AddMethod($"{fieldedEnum.Name}Impl", "ReadFrom", read =>
                    {
                        var stream = new Variable("stream");
                        read.Public().Static()
                            .WithParameters(new ParameterReference(typeof(MemoryStream), "stream"))
                            .DeclareVariable("type",
                                new CastExpression(fieldedEnum.Name,
                                    new GetField(new TypeCall(fieldedEnum.SubType, "ReadFrom", stream), "Value")))
                            .DeclareVariable($"I{fieldedEnum.Name}?", "data", new Default($"I{fieldedEnum.Name}"));
                        // TODO: Make this a switch statement
                        var type = new Variable("type");
                        var data = new Variable("data");
                        foreach (var overloaded in fieldedEnum.OverloadedFields)
                        {
                            var field = overloaded.Key;
                            var ty = overloaded.Value;
                            var comparison = new Equals(type, new GetStatic(fieldedEnum.Name, field));
                            read.If(comparison, x => x.Add(new Assignment(data, new TypeCall(ty, "ReadFrom", stream))));
                        }

                        read.Return(new NewObject().InitializeWith(init =>
                            init.SetField("Type", type).SetField("Data", data)));
                    })
                    .AddMethod("void", "WriteTo",
                        write => write.Public()
                            .WithParameters(new ParameterReference(typeof(MemoryStream), "stream"))
                            .Add(new Call(
                                new CastExpression(fieldedEnum.SubType,
                                    new CastExpression("int", new Variable("Type"))), "WriteTo",
                                new Variable("stream")))
                            .Add(new Call(new NullPropagate(new Variable("Data")), "Write",
                                new Variable("stream"))))
            );
        context.AddSource($"{fieldedEnum.Namespace}.{fieldedEnum.Name}.g.cs", sfb.Build());
    }

    private static void GeneratePacketParser(GeneratorExecutionContext context,
        Dictionary<string, Dictionary<int, string>> serverBoundPackets)
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

    private static void RegisterFieldedEnum(AttributeSyntax attr, SemanticModel semanticModel,
        Dictionary<string, FieldedEnumInformation> fieldedEnums,
        string enumName, string namespaceName, string[] usings)
    {
        var descendantTokens = attr.DescendantTokens().ToList();
        var identNode = descendantTokens.LastOrDefault(dt => dt.IsKind(SyntaxKind.IdentifierToken));
        var primitiveType = semanticModel.GetTypeInfo(identNode.Parent!).Type!.Name;
        if (!fieldedEnums.TryGetValue(enumName, out var fieldedEnum))
            fieldedEnum = fieldedEnums[enumName] = new()
            {
                Namespace = namespaceName,
                Name = enumName,
                Usings = usings.ToList(),
                OverloadedFields = []
            };
    }

    private void GenerateEnumFieldImplementation(GeneratorExecutionContext context, AttributeSyntax enumFieldAttr,
        SemanticModel semanticModel, string className, Dictionary<string, FieldedEnumInformation> fieldedEnums,
        string namespaceName, string[] usings,
        ClassDeclarationSyntax clazz)
    {
        var descendantTokens = enumFieldAttr.DescendantTokens().ToList();
        var identNode = descendantTokens.LastOrDefault(dt => dt.IsKind(SyntaxKind.IdentifierToken));
        var relatedType = semanticModel.GetTypeInfo(identNode.Parent!).Type!.Name;

        var fieldName = className.StartsWith(relatedType)
            ? className.Substring(relatedType.Length)
            : className;
        if (!fieldedEnums.TryGetValue(relatedType, out var enumeration))
            enumeration = fieldedEnums[relatedType] = new FieldedEnumInformation
            {
                Namespace = namespaceName,
                Name = relatedType,
            };
        enumeration.Usings.Add(namespaceName);
        enumeration.Usings.AddRange(usings);
        enumeration.OverloadedFields[fieldName] = $"{namespaceName}.{className}";

        var sfb = StartEnumFieldClass(namespaceName, className, relatedType,
            usings.Append(enumeration.Namespace), out var @class);

        var read = StartReadFrom(@class, className);
        var write = StartWrite(@class);
        List<string> fields = [];
        // Now let's go over every field
        foreach (var field in clazz.DescendantNodes().OfType<FieldDeclarationSyntax>())
        {
            GeneratePacketFieldImpl(field, semanticModel, read, write, fields);
        }

        EndReadFrom(read, fields.ToArray());
        // Now we add a conversion operator
        @class.AddConversionOperator($"{relatedType}Impl",
            method => method.Public().Implicit().WithParameters(new ParameterReference(className, "value"))
                .Return(new NewObject().InitializeWith(init =>
                    init.SetField("Type", new GetStatic(relatedType, fieldName))
                        .SetField("Data", new Variable("value")))));

        context.AddSource($"{namespaceName}.{className}.g.cs", sfb.Build());
    }

    private void GenerateSubPacketImplementation(GeneratorExecutionContext context, string namespaceName,
        string structName,
        string[] usings, StructDeclarationSyntax structure, SemanticModel semanticModel)
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

    private void GeneratePacketImplementation(GeneratorExecutionContext context, AttributeSyntax attr,
        string namespaceName,
        string className, string[] usings, ClassDeclarationSyntax clazz, SemanticModel semanticModel,
        Dictionary<string, Dictionary<int, string>> serverBoundPackets)
    {
        var descendentTokens = attr.DescendantTokens().ToArray();
        var byteNode = descendentTokens.FirstOrDefault(x => x.IsKind(SyntaxKind.NumericLiteralToken));
        if (byteNode == null) return;
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
                a.DescendantTokens().Any(dt => CheckForAttribute(dt, semanticModel, "PacketField"))) is { } fieldAttr)
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
                a.DescendantTokens().Any(dt => CheckForAttribute(dt, semanticModel, "PacketEnum"))) is
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
                a.DescendantTokens().Any(dt => CheckForAttribute(dt, semanticModel, "PacketArrayField"))) is
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
                a.DescendantTokens().Any(dt => CheckForAttribute(dt, semanticModel, "PacketOptionalField"))) is
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

    private bool CheckForAttribute(SyntaxToken token, SemanticModel model, string attr)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        var name = model.GetTypeInfo(token.Parent!).Type?.Name;
        return name == attr;
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

    private SourceFileBuilder StartEnumFieldClass(string ns, string className, string enumName,
        IEnumerable<string> usings, out StructuredTypeReference @class)
    {
        var sfb = new SourceFileBuilder().WithFileScopedNamespace(ns).Using(usings).AddClass(className, out @class);
        @class.Partial().Inherit($"I{enumName}");
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