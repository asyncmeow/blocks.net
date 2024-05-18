using System.Reflection;
using System.Text;
using Blocks.Net.LibSourceGeneration.Builders;
using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.Expressions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.Query;
using Blocks.Net.LibSourceGeneration.References;
using Blocks.Net.LibSourceGeneration.Statements;
using Blocks.Net.PacketSourceGenerator.Attributes;
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
        { "System.Boolean", "Blocks.Net.Packets.Primitives.Boolean" },
        { "System.Byte", "UnsignedByte" },
        { "System.SByte", "SignedByte" },
        { "System.Int16", "Short" },
        { "System.UInt16", "UnsignedShort" },
        { "System.Int32", "Int" },
        { "System.UInt32", "UnsignedInt" },
        { "System.Int64", "Long" },
        { "System.UInt64", "UnsignedLong" },
        { "System.Single", "Float" },
        { "System.Double", "Blocks.Net.Packets.Primitives.Double" },
        { "System.String", "Blocks.Net.Packets.Primitives.String" }
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
        SyntaxAssembly assembly = new(context);
        Dictionary<string, Dictionary<int, string>> serverBoundPackets = [];
        Dictionary<string, FieldedEnumInformation> fieldedEnums = [];
        var foundPacketParser = false;

        foreach (var type in assembly.Types)
        {
            if (type.FullName == "Blocks.Net.Packets.PacketParser") foundPacketParser = true;
            if (type.GetAttributes<Packet>().FirstOrDefault() is { } packet)
            {
                GeneratePacketImplementation(context, type, packet, serverBoundPackets);
            }

            if (type.HasAttribute<SubPacket>())
            {
                GenerateSubPacketImplementation(context, type);
            }

            if (type.GetAttributes<EnumField>().FirstOrDefault() is { } enumField)
            {
                GenerateEnumFieldImplementation(context, type, enumField, fieldedEnums);
            }

            if (type.GetAttributes<FieldedEnum>().FirstOrDefault() is { } fieldedEnum)
            {
                RegisterFieldedEnum(type, fieldedEnums, fieldedEnum);
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
            .Using("Blocks.Net.Packets.Primitives")
            .Nullable()
            .AddInterface($"I{fieldedEnum.Name}",
                enumInterface => enumInterface.Public()
                    .AddMethod("void", "Write",
                        method => method.WithoutImplementation()
                            .WithParameters(new ParameterReference(typeof(MemoryStream), "stream")).Public())
                    .AddMethod($"I{fieldedEnum.Name}?", "ReadFrom", read =>
                    {
                        read.Public().Static()
                            .WithParameters(new ParameterReference(typeof(MemoryStream), "stream"),
                                new ParameterReference(fieldedEnum.Name, "type"))
                            .DeclareVariable($"I{fieldedEnum.Name}?", "data", new Default($"I{fieldedEnum.Name}"));
                        var stream = new Variable("stream");
                        var type = new Variable("type");
                        var data = new Variable("data");
                        // TODO: Make this a switch statement
                        foreach (var overloaded in fieldedEnum.OverloadedFields)
                        {
                            var field = overloaded.Key;
                            var ty = overloaded.Value;
                            var comparison = new Equals(type, new GetStatic(fieldedEnum.Name, field));
                            read.If(comparison, x => x.Add(new Assignment(data, new TypeCall(ty, "ReadFrom", stream))));
                        }

                        read.Return(data);
                    }))
            .AddStruct($"{fieldedEnum.Name}Impl",
                enumStruct => enumStruct.Public()
                    .AddField(fieldedEnum.Name, "Type", type => type.Public())
                    .AddField($"I{fieldedEnum.Name}?", "Data", value => value.Public())
                    .AddMethod($"{fieldedEnum.Name}Impl", "ReadFrom", read =>
                    {
                        var stream = new Variable("stream");
                        var type = new Variable("type");
                        var data = new Variable("data");
                        read.Public().Static()
                            .WithParameters(new ParameterReference(typeof(MemoryStream), "stream"))
                            .DeclareVariable("type",
                                new CastExpression(fieldedEnum.Name,
                                    new GetField(new TypeCall(fieldedEnum.SubType, "ReadFrom", stream), "Value")))
                            .DeclareVariable($"I{fieldedEnum.Name}?", "data",
                                new TypeCall($"I{fieldedEnum.Name}", "ReadFrom", stream, type));
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
                    .AddConversionOperator($"{fieldedEnum.Name}Impl", convert => convert.Public().Implicit()
                        .WithParameters(new ParameterReference(fieldedEnum.Name, "type")).Return(
                            new NewObject().InitializeWith(
                                init => init.SetField("Type", new Variable("type")))))
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

    private static void RegisterFieldedEnum(SyntaxType type, Dictionary<string, FieldedEnumInformation> fieldedEnums,
        FieldedEnum attr)
    {
        if (!fieldedEnums.TryGetValue(type.Name, out var fieldedEnum))
            fieldedEnum = fieldedEnums[type.Name] = new();
        fieldedEnum.Namespace = type.Module.Namespace;
        fieldedEnum.Name = type.Name;
        fieldedEnum.Usings.AddRange(type.Module.Usings);
        var primType = attr.PrimitiveType.FullName!;
        primType = PrimitiveRemap.TryGetValue(primType, out var p2) ? p2 : primType;
        fieldedEnum.SubType = primType!;
    }

    private void GenerateEnumFieldImplementation(GeneratorExecutionContext context, SyntaxType type, EnumField attr,
        Dictionary<string, FieldedEnumInformation> fieldedEnums)
    {
        var className = type.Name;
        var relatedType = attr.Enumeration.Name;
        var sfb = new SourceFileBuilder().Nullable().Using("Blocks.Net.Packets.Primitives");
        ;
        var impl = type.GenerateImplementation(sfb).Inherit($"I{relatedType}");
        var fieldName = className.StartsWith(relatedType)
            ? className.Substring(relatedType.Length)
            : className;
        if (!fieldedEnums.TryGetValue(relatedType, out var enumeration))
            enumeration = fieldedEnums[relatedType] = new FieldedEnumInformation
            {
                Namespace = type.Module.Namespace,
                Name = relatedType,
            };
        enumeration.Usings.Add(type.Module.Namespace);
        enumeration.Usings.AddRange(type.Module.Usings);
        enumeration.OverloadedFields.Add(fieldName, type.FullName);

        var read = StartReadFrom(impl, className);
        var write = StartWrite(impl);
        List<string> fields = [];

        foreach (var field in type.Fields)
        {
            GeneratePacketFieldImpl(field, read, write, fields);
        }

        EndReadFrom(read, fields.ToArray());
        impl.AddConversionOperator($"{relatedType}Impl",
            method => method.Public().Implicit().WithParameters(new ParameterReference(className, "value"))
                .Return(new NewObject().InitializeWith(init =>
                    init.SetField("Type", new GetStatic(relatedType, fieldName))
                        .SetField("Data", new Variable("value")))));

        context.AddSource($"{type.FullName}.g.cs", sfb.Build());
    }

    private void GenerateSubPacketImplementation(GeneratorExecutionContext context, SyntaxType subPacketType)
    {
        var sfb = new SourceFileBuilder().Nullable().Using("Blocks.Net.Packets.Primitives");
        ;
        var impl = subPacketType.GenerateImplementation(sfb);
        var read = StartReadFrom(impl, subPacketType.Name, true);
        var write = StartWrite(impl, true);
        List<string> fields = [];
        foreach (var field in subPacketType.Fields)
        {
            GeneratePacketFieldImpl(field, read, write, fields);
        }

        EndReadFrom(read, fields.ToArray());
        context.AddSource($"{subPacketType.FullName}.g.cs", sfb.Build());
    }

    private void GeneratePacketImplementation(GeneratorExecutionContext context, SyntaxType packetType, Packet attr,
        Dictionary<string, Dictionary<int, string>> serverBoundPackets)
    {
        var builder = new SourceFileBuilder().Nullable().Using("Blocks.Net.Packets.Primitives");
        var impl = packetType.GenerateImplementation(builder).AddProperty("int", "PacketId",
            id => id.Public()
                .AddGetter(get => get.Return(new IntegerLiteral(attr.Id, @base: IntegerBase.Hexadecimal))));

        var read = StartReadFrom(impl, packetType.Name);
        var write = StartWrite(impl);
        List<string> fields = [];
        foreach (var field in packetType.Fields)
        {
            GeneratePacketFieldImpl(field, read, write, fields);
        }

        EndReadFrom(read, fields.ToArray());
        if (!attr.ClientBound)
        {
            var dict = serverBoundPackets.TryGetValue(attr.State, out var d)
                ? d
                : serverBoundPackets[attr.State] = [];
            dict[attr.Id] = packetType.FullName;
        }

        context.AddSource($"{packetType.FullName}.g.cs", builder.Build());
    }

    private void GeneratePacketFieldImpl(SyntaxField field, MethodReference read, MethodReference write,
        List<string> fields)
    {
        var targetType = field.Type;
        var name = field.Name;
        if (field.HasAttribute<PacketField>())
        {
            var primitiveType = PrimitiveRemap.TryGetValue(targetType, out var primType)
                ? primType
                : targetType;
            AddSimpleFieldRead(read, name, targetType, primitiveType);
            AddSimpleFieldWrite(write, name, targetType, primitiveType);
            fields.Add(name);
        }
        else if (field.GetAttributes<PacketEnum>().FirstOrDefault() is { } enumAttr)
        {
            var relatedType = enumAttr.EnumType.FullName;
            var primitiveType = PrimitiveRemap.TryGetValue(relatedType, out var primType)
                ? primType
                : relatedType;
            AddEnumFieldRead(read, name, targetType, primitiveType);
            AddEnumFieldWrite(write, name, primitiveType);
            fields.Add(name);
        }
        else if (field.GetAttributes<PacketArrayField>().FirstOrDefault() is { } packetArrayField)
        {
            var targetTypeSubType = targetType.Substring(0, targetType.Length - 2);
            var primitiveSubType = PrimitiveRemap.TryGetValue(targetTypeSubType, out var primSubType)
                ? primSubType
                : targetTypeSubType;
            AddArrayFieldRead(read, name, targetTypeSubType, primitiveSubType, packetArrayField.ArraySizeControl);
            AddArrayFieldWrite(write, name, targetTypeSubType, primitiveSubType, packetArrayField.ArraySizeControl);
            fields.Add(name);
        }
        else if (field.GetAttributes<PacketOptionalField>().FirstOrDefault() is { } packetOptionalField)
        {
            var primitiveType = PrimitiveRemap.TryGetValue(targetType, out var primType)
                ? primType
                : targetType;
            AddOptionalFieldRead(read, name, targetType, primitiveType, packetOptionalField.ControllingCondition);
            AddOptionalFieldWrite(write, name, targetType, primitiveType, packetOptionalField.ControllingCondition);
            fields.Add(name);
        }
        else if (field.GetAttributes<PacketSplitEnumDataField>().FirstOrDefault() is { } packetSplitEnumDataField)
        {
            AddSplitEnumFieldRead(read, name, targetType, packetSplitEnumDataField.EnumControl);
            AddSplitEnumFieldWrite(write, name);
            fields.Add(name);
        }
        else if (field.GetAttributes<PacketOptionalArrayField>().FirstOrDefault() is { } packetOptionalArrayField)
        {
            var targetTypeSubType = targetType.Substring(0, targetType.Length - 2);
            var primitiveSubType = PrimitiveRemap.TryGetValue(targetTypeSubType, out var primSubType)
                ? primSubType
                : targetTypeSubType;
            AddOptionalArrayFieldRead(read, name, targetTypeSubType, primitiveSubType,
                packetOptionalArrayField.ArraySizeControl, packetOptionalArrayField.ControllingCondition);
            AddOptionalArrayFieldWrite(write, name, targetTypeSubType, primitiveSubType,
                packetOptionalArrayField.ArraySizeControl, packetOptionalArrayField.ControllingCondition);
            fields.Add(name);
        }
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

    private void AddSplitEnumFieldRead(MethodReference method, string fieldName, TypeReference enumDataType,
        string control) =>
        method.DeclareVariable(fieldName,
            new TypeCall(enumDataType, "ReadFrom", new Variable("stream"), new InjectedExpression(control)));

    private void AddOptionalArrayFieldRead(MethodReference method, string fieldName, TypeReference targetSubType,
        TypeReference primitiveSubType, string lengthControl, string optionalControl)
    {
        var lengthVarName = $"__{fieldName}_length__";
        var iterVarName = $"__{fieldName}_iter__";
        method.DeclareVariable(fieldName, new Default(targetSubType.MakeArray(1)));
        method.If(new InjectedExpression(optionalControl), block =>
        {
            block.DeclareVariable(typeof(int), lengthVarName, new InjectedExpression(lengthControl));
            var lengthVar = new Variable(lengthVarName);
            var iterVar = new Variable(iterVarName);
            var field = new Variable(fieldName);
            block.Add(new Assignment(fieldName, new NewArray(targetSubType, lengthVar)));
            var call = new TypeCall(primitiveSubType, "ReadFrom", new Variable("stream"));
            var init = primitiveSubType == targetSubType ? (IExpression)call : new CastExpression(targetSubType, call);
            var assign = new Assignment(new Subscript(field, iterVar), init);
            block.For(new VariableDeclarationStatement(iterVarName, new IntegerLiteral(0)),
                new LessThan(iterVar, lengthVar), new PostIncrement(iterVar),
                loop => loop.Add(assign));
        });
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

    private void AddSplitEnumFieldWrite(MethodReference method, string fieldName) =>
        method.Add(new Call(new NullPropagate(new Variable(fieldName)), "Write", new Variable("stream")));
    private void AddOptionalArrayFieldWrite(MethodReference method, string fieldName, TypeReference targetSubType,
        TypeReference primitiveSubType, string lengthControl, string optionalControl)
    {
        method.If(new InjectedExpression(optionalControl), block =>
        {
            var lengthVarName = $"__{fieldName}_length__";
            var iterVarName = $"__{fieldName}_iter__";
            block.DeclareVariable(typeof(int), lengthVarName, new InjectedExpression(lengthControl));
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
            block.For(new VariableDeclarationStatement(iterVarName, new IntegerLiteral(0)),
                new LessThan(iterVar, lengthVar), new PostIncrement(iterVar),
                loop => loop.Add(call));

        });
    }
}