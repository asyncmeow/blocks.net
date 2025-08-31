using System.Reflection;
using System.Reflection.Metadata;
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
using TypeReference = Blocks.Net.LibSourceGeneration.References.TypeReference;

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
        { "RegistryReference", "Blocks.Net.Packets.Primitives.VarInt" },
        { "BlockState", "Blocks.Net.Packets.Primitives.VarInt" },
        { "ItemId", "Blocks.Net.Packets.Primitives.VarInt" },
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
        { "System.String", "Blocks.Net.Packets.Primitives.String" },
        { "NamespacedIdentifier", "Blocks.Net.Packets.Primitives.Identifier" },
        { "Blocks.Net.Framework.Primitives.NamespacedIdentifier", "Blocks.Net.Packets.Primitives.Identifier" },
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
        var foundPacketState = false;
        Dictionary<string, TypeReference> packetStateFields = new();

        foreach (var type in assembly.Types)
        {
            if (type.FullName == "Blocks.Net.Packets.PacketParser") foundPacketParser = true;
            if (type.FullName == "Blocks.Net.Packets.PacketState") foundPacketState = true;
            if (type.GetAttributes<Packet>().FirstOrDefault() is { } packet)
            {
                GeneratePacketImplementation(context, type, packet, serverBoundPackets);
            }

            if (type.GetAttributes<SubPacket>().FirstOrDefault() is { } subPacket)
            {
                GenerateSubPacketImplementation(context, type, subPacket);
            }

            if (type.GetAttributes<EnumField>().FirstOrDefault() is { } enumField)
            {
                GenerateEnumFieldImplementation(context, type, enumField, fieldedEnums);
            }

            if (type.GetAttributes<FieldedEnum>().FirstOrDefault() is { } fieldedEnum)
            {
                RegisterFieldedEnum(type, fieldedEnums, fieldedEnum);
            }

            if (type.GetAttributes<GenerateIdOrXFor>().FirstOrDefault() is not null)
            {
                GenerateIdOrInlineFor(context, type);
            }

            foreach (var attr in type.GetAttributes<RequiresStateField>())
            {
                packetStateFields[attr.FieldName] = attr.FieldType;
            }
        }

        if (foundPacketParser)
        {
            GeneratePacketParser(context, serverBoundPackets);
        }

        if (foundPacketState)
        {
            GeneratePacketState(context, packetStateFields);
        }

        foreach (var fieldedEnum in fieldedEnums.Values)
        {
            BuildFieldedEnum(context, fieldedEnum);
        }
    }

    private void GenerateIdOrInlineFor(GeneratorExecutionContext context, SyntaxType type)
    {
        var builder = new SourceFileBuilder().Using("Blocks.Net.DataTypes").Using("Blocks.Net.Packets.Primitives")
            .WithFileScopedNamespace(type.Module.Namespace).AddStruct($"IdOr{type.Name}",
                struc =>
                {
                    struc.Public();
                    struc.AddField("VarInt", "Id", field => field.Public());
                    struc.AddField(type.ShortReference.AsNullable(), "Value", field => field.Public());
                    struc.AddProperty("bool", "IsInline",
                        property => property.Public().AddGetter(get =>
                            get.Return(new Equals(new Variable("Id"), new IntegerLiteral(0)))));
                    struc.AddProperty("RegistryReference", "RegistryId",
                        property => property.Public().AddGetter(get =>
                            get.Return(new NewObject(null,
                                new InjectedExpression(
                                    "Id == 0 ? throw new Exception(\"Attempting to get the registry reference for an inline definition\") : Id - 1")))));
                    struc.AddConversionOperator("RegistryReference",
                        method => method.Implicit().WithParameters(new ParameterReference($"IdOr{type.Name}", "cur"))
                            .Public().Return(new GetField(new Variable("cur"), "RegistryId")));
                    struc.AddConversionOperator(type.ShortReference,
                        method => method.Public().Implicit()
                            .WithParameters(new ParameterReference($"IdOr{type.Name}", "cur"))
                            .Return(new InjectedExpression(
                                "cur.Value ?? throw new Exception(\"Inline definition is null!\")")));
                    struc.AddConversionOperator($"IdOr{type.Name}",
                        method => method.Implicit().WithParameters(new ParameterReference("RegistryReference", "from"))
                            .Public().Return(new NewObject(null, new Variable("from"))));
                    struc.AddConversionOperator($"IdOr{type.Name}",
                        method => method.Implicit().WithParameters(new ParameterReference(type.ShortReference, "from"))
                            .Public().Return(new NewObject(null, new Variable("from"))));
                    struc.AddMethod("void", "WriteTo", write =>
                    {
                        write.Public();
                        write.WithParameters(_stream, _state);
                        write.Add(new Call(new Variable("Id"), "WriteTo", _streamVar, _stateVar));
                        write.If(new Equals(new Variable("Id"), new IntegerLiteral(0)),
                            @if =>
                            {
                                @if.Add(new Call(new NullPropagate(new Variable("Value")), "WriteTo", _streamVar,
                                    _stateVar));
                            });
                    });

                    struc.AddMethod($"IdOr{type.Name}", "ReadFrom", read =>
                    {
                        read.Public().Static();
                        read.WithParameters(_stream, _state);
                        read.DeclareVariable("id", new TypeCall("VarInt", "ReadFrom", _streamVar, _stateVar));
                        read.If(new Equals(new Variable("id"), new IntegerLiteral(0)), @if =>
                        {
                            @if.Return(new NewObject(null,
                                new TypeCall(type.ShortReference, "ReadFrom", _streamVar, _stateVar)));
                            @if.Else(@else => @else.Return(new NewObject(null, new Variable("id"))));
                        });
                    });

                    struc.AddConstructor(method =>
                    {
                        method.WithParameters(new ParameterReference("VarInt", "id"));
                        method.Add(new Assignment("Id", new Variable("id")));
                        method.Add(new Assignment("Value", new Variable("null")));
                    });


                    struc.AddConstructor(method =>
                    {
                        method.Public();
                        method.WithParameters(new ParameterReference("RegistryReference", "id"));
                        method.Add(new Assignment("Id",
                            new NewObject(null,
                                new Addition(new GetField(new Variable("id"), "RegistryId"), new IntegerLiteral(1)))));
                        method.Add(new Assignment("Value", new Variable("null")));
                    });

                    struc.AddConstructor(method =>
                    {
                        method.Public();
                        method.WithParameters(new ParameterReference(type.ShortReference, "value"));
                        method.Add(new Assignment("Id", new IntegerLiteral(0)));
                        method.Add(new Assignment("Value", new Variable("value")));
                    });
                });

        context.AddSource($"IdOr{type.Name}.g.cs", builder.Build());
    }

    private static void GeneratePacketState(GeneratorExecutionContext context,
        Dictionary<string, TypeReference> packetStateFields)
    {
        var builder = new SourceFileBuilder().WithFileScopedNamespace("Blocks.Net.Packets").AddClass("PacketState",
            @class =>
            {
                @class.Partial();
                foreach (var kvp in packetStateFields)
                {
                    @class.AddField(kvp.Value, kvp.Key, field => field.Public());
                }
            });
        context.AddSource("PacketState.g.cs", builder.Build());
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
                            .WithParameters(_stream, _state).Public())
                    .AddMethod($"I{fieldedEnum.Name}?", "ReadFrom", read =>
                    {
                        read.Public().Static()
                            .WithParameters(_stream, _state, new ParameterReference(fieldedEnum.Name, "type"))
                            .DeclareVariable($"I{fieldedEnum.Name}?", "data", new Default($"I{fieldedEnum.Name}"));
                        var type = new Variable("type");
                        var data = new Variable("data");
                        // TODO: Make this a switch statement
                        foreach (var overloaded in fieldedEnum.OverloadedFields)
                        {
                            var field = overloaded.Key;
                            var ty = overloaded.Value;
                            var comparison = new Equals(type, new GetStatic(fieldedEnum.Name, field));
                            read.If(comparison,
                                x => x.Add(new Assignment(data, new TypeCall(ty, "ReadFrom", _streamVar, _stateVar))));
                        }

                        read.Return(data);
                    }))
            .AddStruct($"{fieldedEnum.Name}Impl",
                enumStruct => enumStruct.Public()
                    .AddField(fieldedEnum.Name, "Type", type => type.Public())
                    .AddField($"I{fieldedEnum.Name}?", "Data", value => value.Public())
                    .AddMethod($"{fieldedEnum.Name}Impl", "ReadFrom", read =>
                    {
                        var type = new Variable("type");
                        var data = new Variable("data");
                        read.Public().Static()
                            .WithParameters(_stream, _state)
                            .DeclareVariable("type",
                                new CastExpression(fieldedEnum.Name,
                                    new GetField(new TypeCall(fieldedEnum.SubType, "ReadFrom", _streamVar, _stateVar),
                                        "Value")))
                            .DeclareVariable($"I{fieldedEnum.Name}?", "data",
                                new TypeCall($"I{fieldedEnum.Name}", "ReadFrom", _streamVar, _stateVar, type));
                        read.Return(new NewObject().InitializeWith(init =>
                            init.SetField("Type", type).SetField("Data", data)));
                    })
                    .AddMethod("void", "WriteTo",
                        write => write.Public()
                            .WithParameters(_stream, _state)
                            .Add(new Call(
                                new CastExpression(fieldedEnum.SubType,
                                    new CastExpression("int", new Variable("Type"))), "WriteTo",
                                _streamVar, _stateVar))
                            .Add(new Call(new NullPropagate(new Variable("Data")), "Write",
                                _streamVar, _stateVar)))
                    .AddConversionOperator($"{fieldedEnum.Name}Impl", convert => convert.Public().Implicit()
                        .WithParameters(new ParameterReference(fieldedEnum.Name, "type")).Return(
                            new NewObject().InitializeWith(init => init.SetField("Type", new Variable("type")))))
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
                    @class.AddField("Dictionary<int,Func<Stream,PacketState,IPacket>>", $"{state}ServerBoundPackets",
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

    private void GenerateSubPacketImplementation(GeneratorExecutionContext context, SyntaxType subPacketType,
        SubPacket subPacket)
    {
        var sfb = new SourceFileBuilder().Nullable().Using("Blocks.Net.Packets.Primitives");
        var impl = subPacketType.GenerateImplementation(sfb);
        var read = StartReadFrom(impl, subPacketType.Name, true);
        read.WithParameters(subPacket.ExtraArgs.Select((x, i) => new ParameterReference(x, $"_{i}")).ToArray());
        var write = StartWrite(impl, true);
        write.WithParameters(subPacket.ExtraArgs.Select((x, i) => new ParameterReference(x, $"_{i}")).ToArray());
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

        var impl = packetType.GenerateImplementation(builder).AddField("int", "PACKET_ID",
            field => field.Public().Const()
                .Default(new IntegerLiteral(attr.Id, @base: IntegerBase.Hexadecimal))
        ).AddProperty("int", "PacketId",
            id => id.Public()
                .AddGetter(get => get.Return(new Variable("PACKET_ID"))));
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
        if (field.GetAttributes<PacketField>().FirstOrDefault() is { } fieldAttr)
        {
            int mode = 0;
            if (targetType.EndsWith("?[]?"))
            {
                mode = 1;
                targetType = targetType.Replace("?[]?", string.Empty);
            }
            else if (targetType.EndsWith("[]?"))
            {
                mode = 2;
                targetType = targetType.Replace("[]?", string.Empty);
            }
            else if (targetType.EndsWith("?[]"))
            {
                mode = 3;
                targetType = targetType.Replace("?[]", string.Empty);
            }
            else if (targetType.EndsWith("[]"))
            {
                mode = 4;
                targetType = targetType.Replace("[]", string.Empty);
            }
            else if (targetType.EndsWith("?"))
            {
                mode = 5;
                targetType = targetType.Replace("?", string.Empty);
            }

            var primitiveType = PrimitiveRemap.TryGetValue(targetType, out var primType)
                ? primType
                : targetType;
            switch (mode)
            {
                case 0:
                    AddSimpleFieldRead(read, name, targetType, primitiveType, fieldAttr.InjectedArguments);
                    AddSimpleFieldWrite(write, name, targetType, primitiveType, fieldAttr.InjectedArguments);
                    break;
                case 1:
                    AddInlineOptionalArrayOfOptionalFieldRead(read, name, targetType, primitiveType,
                        fieldAttr.InjectedArguments);
                    AddInlineOptionalArrayOfOptionalFieldWrite(write, name, targetType, primitiveType,
                        fieldAttr.InjectedArguments);
                    break;
                case 2:
                    AddInlineOptionalArrayFieldRead(read, name, targetType, primitiveType, fieldAttr.InjectedArguments);
                    AddInlineOptionalArrayFieldWrite(write, name, targetType, primitiveType,
                        fieldAttr.InjectedArguments);
                    break;
                case 3:
                    AddInlineArrayOfOptionalFieldRead(read, name, targetType, primitiveType,
                        fieldAttr.InjectedArguments);
                    AddInlineArrayOfOptionalFieldWrite(write, name, targetType, primitiveType,
                        fieldAttr.InjectedArguments);
                    break;
                case 4:
                    AddInlineArrayFieldRead(read, name, targetType, primitiveType, fieldAttr.InjectedArguments);
                    AddInlineArrayFieldWrite(write, name, targetType, primitiveType,
                        fieldAttr.InjectedArguments);
                    break;
                case 5:
                    AddInlineOptionalFieldRead(read, name, targetType, primitiveType,
                        fieldAttr.InjectedArguments);
                    AddInlineOptionalFieldWrite(write, name, targetType, primitiveType,
                        fieldAttr.InjectedArguments);
                    break;
            }

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
            AddArrayFieldRead(read, name, targetTypeSubType, primitiveSubType, packetArrayField.ArraySizeControl,
                packetArrayField.InjectedArgs);
            AddArrayFieldWrite(write, name, targetTypeSubType, primitiveSubType, packetArrayField.ArraySizeControl,
                packetArrayField.InjectedArgs);
            fields.Add(name);
        }
        else if (field.GetAttributes<PacketOptionalField>().FirstOrDefault() is { } packetOptionalField)
        {
            var primitiveType = PrimitiveRemap.TryGetValue(targetType, out var primType)
                ? primType
                : targetType;
            AddOptionalFieldRead(read, name, targetType, primitiveType, packetOptionalField.ControllingCondition,
                packetOptionalField.InjectedArgs);
            AddOptionalFieldWrite(write, name, targetType, primitiveType, packetOptionalField.ControllingCondition,
                packetOptionalField.InjectedArgs);
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
                packetOptionalArrayField.ArraySizeControl, packetOptionalArrayField.ControllingCondition,
                packetOptionalArrayField.InjectedArgs);
            AddOptionalArrayFieldWrite(write, name, targetTypeSubType, primitiveSubType,
                packetOptionalArrayField.ArraySizeControl, packetOptionalArrayField.ControllingCondition,
                packetOptionalArrayField.InjectedArgs);
            fields.Add(name);
        }
    }

    private static ParameterReference _stream = new ParameterReference(typeof(Stream), "stream");
    private static Variable _streamVar = new Variable("stream");
    private static ParameterReference _state = new ParameterReference("Blocks.Net.Packets.PacketState", "state");
    private static Variable _stateVar = new Variable("state");

    private MethodReference StartReadFrom(StructuredTypeReference ty, string className, bool isSubPacket = false)
    {
        ty.AddMethod(className, "ReadFrom", out var method);
        return method.Public().Static().WithParameters(_stream, _state)
            .WithDocumentation(new DocCommentBuilder()
                .WithSummary($"Reads a {className} {(isSubPacket ? "sub" : "")}packet from a memory stream\n")
                .WithParameter("stream",
                    $"The {(isSubPacket ? "sub" : "")}packet stream{(isSubPacket ? "" : ", the length and id must already be consumed")}")
                .Returns($"A {(isSubPacket ? "sub" : "")}packet constructed from the given stream"));
    }

    private void AddSimpleFieldRead(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType, IEnumerable<string> injectedArguments)
    {
        var call = new TypeCall(primitiveType, "ReadFrom",
            [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]);
        IExpression init = primitiveType == targetType ? call : new CastExpression(targetType, call);
        method.DeclareVariable(fieldName, init);
    }

    private void AddInlineOptionalFieldRead(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType, IEnumerable<string> injectedArguments)
    {
        var field = new Variable(fieldName);
        method.DeclareVariable(targetType.AsNullable(), fieldName, new Variable("default"));
        method.If(new Equals(new Call(_streamVar, "ReadByte"), new IntegerLiteral(1)), statement =>
        {
            var call = new TypeCall(primitiveType, "ReadFrom",
                [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]);
            statement.Add(primitiveType == targetType
                ? new Assignment(field, call)
                : new Assignment(field, new CastExpression(targetType, call)));
        });
    }

    private void AddInlineArrayFieldRead(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType, IEnumerable<string> injectedArguments)
    {
        var field = new Variable(fieldName);
        var length = new Variable($"__{fieldName}Length");
        var iter = new Variable($"__{fieldName}Iter");
        method.DeclareVariable("int", $"__{fieldName}Length",
            new TypeCall("VarInt", "ReadFrom", _streamVar, _stateVar));
        method.DeclareVariable(targetType.MakeArray(1), fieldName, new NewArray(targetType, length));
        method.For(new VariableDeclarationStatement($"__{fieldName}Iter", new IntegerLiteral(0)),
            new LessThan(iter, length),
            new PostIncrement(iter),
            loop =>
            {
                var call = new TypeCall(primitiveType, "ReadFrom",
                    [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]);
                loop.Add(new Assignment(new Subscript(field, iter),
                    targetType == primitiveType ? call : new CastExpression(targetType, call)));
            });
    }

    private void AddInlineOptionalArrayFieldRead(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType, IEnumerable<string> injectedArguments)
    {
        var field = new Variable(fieldName);
        var length = new Variable($"__{fieldName}Length");
        var iter = new Variable($"__{fieldName}Iter");
        method.DeclareVariable(targetType.MakeArray(1), fieldName, new Variable("null"));
        method.If(new Equals(new Call(_streamVar, "ReadByte"), new IntegerLiteral(1)), out var statement);
        statement.DeclareVariable("int", $"__{fieldName}Length",
            new TypeCall("VarInt", "ReadFrom", _streamVar, _stateVar));
        statement.Add(new Assignment(field, new NewArray(targetType, length)));
        statement.For(new VariableDeclarationStatement($"__{fieldName}Iter", new IntegerLiteral(0)),
            new LessThan(iter, length),
            new PostIncrement(iter),
            loop =>
            {
                var call = new TypeCall(primitiveType, "ReadFrom",
                    [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]);
                loop.Add(new Assignment(new Subscript(field, iter),
                    targetType == primitiveType ? call : new CastExpression(targetType, call)));
            });
    }

    private void AddInlineArrayOfOptionalFieldRead(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType, IEnumerable<string> injectedArguments)
    {
        var field = new Variable(fieldName);
        var length = new Variable($"__{fieldName}Length");
        var iter = new Variable($"__{fieldName}Iter");
        method.DeclareVariable("int", $"__{fieldName}Length",
            new TypeCall("VarInt", "ReadFrom", _streamVar, _stateVar));
        method.DeclareVariable(targetType.AsNullable().MakeArray(1), fieldName,
            new NewArray(targetType.AsNullable(), length));
        method.For(new VariableDeclarationStatement($"__{fieldName}Iter", new IntegerLiteral(0)),
            new LessThan(iter, length),
            new PostIncrement(iter),
            loop => loop.If(new Equals(new Call(_streamVar, "ReadByte"), new IntegerLiteral(1)), statement =>
            {
                var call = new TypeCall(primitiveType, "ReadFrom",
                    [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]);
                statement.Add(new Assignment(new Subscript(field, iter),
                    targetType == primitiveType ? call : new CastExpression(targetType, call)));
            }));
    }

    private void AddInlineOptionalArrayOfOptionalFieldRead(MethodReference method, string fieldName,
        TypeReference targetType,
        TypeReference primitiveType, IEnumerable<string> injectedArguments)
    {
        var field = new Variable(fieldName);
        var length = new Variable($"__{fieldName}Length");
        var iter = new Variable($"__{fieldName}Iter");
        method.DeclareVariable(targetType.AsNullable().MakeArray(1), fieldName, new Variable("null"));
        method.If(new Equals(new Call(_streamVar, "ReadByte"), new IntegerLiteral(1)), out var statement);
        statement.DeclareVariable("int", $"__{fieldName}Length",
            new TypeCall("VarInt", "ReadFrom", _streamVar, _stateVar));
        statement.Add(new Assignment(fieldName, new NewArray(targetType.AsNullable(), length)));
        statement.For(new VariableDeclarationStatement($"__{fieldName}Iter", new IntegerLiteral(0)),
            new LessThan(iter, length),
            new PostIncrement(iter),
            loop => loop.If(new Equals(new Call(_streamVar, "ReadByte"), new IntegerLiteral(1)), innerStatement =>
            {
                var call = new TypeCall(primitiveType, "ReadFrom",
                    [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]);
                innerStatement.Add(new Assignment(new Subscript(field, iter),
                    targetType == primitiveType ? call : new CastExpression(targetType, call)));
            }));
    }

    private void AddEnumFieldRead(MethodReference method, string fieldName, TypeReference enumType,
        TypeReference primitiveType)
    {
        var call = new TypeCall(primitiveType, "ReadFrom", _streamVar, _stateVar);
        var value = new GetField(call, "Value");
        var cast = new CastExpression(enumType, value);
        method.DeclareVariable(fieldName, cast);
    }

    private void AddArrayFieldRead(MethodReference method, string fieldName, TypeReference targetSubType,
        TypeReference primitiveSubType, string lengthControl, IEnumerable<string> injectedArguments)
    {
        var lengthVarName = $"__{fieldName}_length__";
        var iterVarName = $"__{fieldName}_iter__";
        method.DeclareVariable(typeof(int), lengthVarName, new InjectedExpression(lengthControl));
        var lengthVar = new Variable(lengthVarName);
        var iterVar = new Variable(iterVarName);
        var field = new Variable(fieldName);
        method.DeclareVariable(fieldName, new NewArray(targetSubType, lengthVar));
        var call = new TypeCall(primitiveSubType, "ReadFrom",
            [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]);
        var init = primitiveSubType == targetSubType ? (IExpression)call : new CastExpression(targetSubType, call);
        var assign = new Assignment(new Subscript(field, iterVar), init);
        method.For(new VariableDeclarationStatement(iterVarName, new IntegerLiteral(0)),
            new LessThan(iterVar, lengthVar), new PostIncrement(iterVar),
            loop => loop.Add(assign));
    }

    private void AddOptionalFieldRead(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType, string optionalControl, IEnumerable<string> injectedArguments)
    {
        method.DeclareVariable(fieldName, new Default(targetType));
        var call = new TypeCall(primitiveType, "ReadFrom",
            [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]);
        var init = primitiveType == targetType ? (IExpression)call : new CastExpression(targetType, call);
        var assign = new Assignment(fieldName, init);
        method.If(new InjectedExpression(optionalControl), statement => statement.Add(assign));
    }

    private void AddSplitEnumFieldRead(MethodReference method, string fieldName, TypeReference enumDataType,
        string control) =>
        method.DeclareVariable(fieldName,
            new TypeCall(enumDataType, "ReadFrom", _streamVar, _stateVar, new InjectedExpression(control)));

    private void AddOptionalArrayFieldRead(MethodReference method, string fieldName, TypeReference targetSubType,
        TypeReference primitiveSubType, string lengthControl, string optionalControl,
        IEnumerable<string> injectedArguments)
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
            var call = new TypeCall(primitiveSubType, "ReadFrom",
                [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]);
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

        return method.Public().WithParameters(_stream, _state);
    }

    private void AddSimpleFieldWrite(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType, IEnumerable<string> injectedArguments)
    {
        var field = new Variable(fieldName);
        method.Add(new Call(targetType == primitiveType ? field : new CastExpression(primitiveType, field), "WriteTo",
            [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]));
    }

    private void AddInlineOptionalFieldWrite(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType, IEnumerable<string> injectedArguments)
    {
        // var call = new TypeCall(primitiveType, "ReadFrom", [new Variable("stream"), ..injectedArguments.Select(x => new InjectedExpression(x))]);
        // var init = primitiveType == targetType ? (IExpression)call : new CastExpression(targetType, call);
        // method.DeclareVariable(fieldName, init);
        var field = new Variable(fieldName);
        var value = new Variable($"__{fieldName}Value");
        method.Add(new IfStatement(new ExtractValueExpression(field, $"__{fieldName}Value")).Add(
                new Call(_streamVar, "WriteByte", new IntegerLiteral(1))).Add(
                new Call(new CastExpression(primitiveType, value), "WriteTo",
                    [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]))
            .Else(els => els.Add(new Call(_streamVar, "WriteByte", new IntegerLiteral(0)))));
    }

    private void AddInlineArrayFieldWrite(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType, IEnumerable<string> injectedArguments)
    {
        var field = new Variable(fieldName);
        var control = new Variable($"__{fieldName}Iter");
        method.Add(new Call(new CastExpression("VarInt", new GetField(field, "Length")), "WriteTo", _streamVar,
            _stateVar));
        method.For(new VariableDeclarationStatement("int", $"__{fieldName}Iter", new IntegerLiteral(0)),
            new LessThan(control, new GetField(field, "Length")), new PostIncrement(control),
            loop => loop.Add(new Call(
                targetType == primitiveType
                    ? new Subscript(field, control)
                    : new CastExpression(primitiveType, new Subscript(field, control)), "WriteTo",
                [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))])));
    }

    private void AddInlineOptionalArrayFieldWrite(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType, IEnumerable<string> injectedArguments)
    {
        var field = new Variable(fieldName);
        var value = new Variable($"__{fieldName}Value");
        var control = new Variable($"__{fieldName}Iter");
        method.Add(new IfStatement(new ExtractValueExpression(field, $"__{fieldName}Value"))
            .Add(new Call(_streamVar, "WriteByte", new IntegerLiteral(1)))
            .Add(new Call(new CastExpression("VarInt", new GetField(value, "Length")), "WriteTo", _streamVar,
                _stateVar))
            .For(new VariableDeclarationStatement("int", $"__{fieldName}Iter", new IntegerLiteral(0)),
                new LessThan(control, new GetField(field, "Length")), new PostIncrement(control),
                loop => loop.Add(new Call(
                    targetType == primitiveType
                        ? new Subscript(value, control)
                        : new CastExpression(primitiveType, new Subscript(value, control)), "WriteTo",
                    [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))])))
            .Else(els => els.Add(new Call(_streamVar, "WriteByte", new IntegerLiteral(0)))));
    }

    private void AddInlineArrayOfOptionalFieldWrite(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType, IEnumerable<string> injectedArguments)
    {
        var field = new Variable(fieldName);
        var value = new Variable($"__{fieldName}Value");
        var control = new Variable($"__{fieldName}Iter");
        method.Add(new Call(new CastExpression("VarInt", new GetField(field, "Length")), "WriteTo", _streamVar,
            _stateVar));
        method.For(new VariableDeclarationStatement("int", $"__{fieldName}Iter", new IntegerLiteral(0)),
            new LessThan(control, new GetField(field, "Length")), new PostIncrement(control),
            loop => loop.Add(
                new IfStatement(new ExtractValueExpression(new Subscript(field, control), $"__{fieldName}Value")).Add(
                        new Call(_streamVar, "WriteByte", new IntegerLiteral(1))).Add(
                        new Call(targetType == primitiveType ? value : new CastExpression(primitiveType, value),
                            "WriteTo",
                            [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]))
                    .Else(els => els.Add(new Call(_streamVar, "WriteByte", new IntegerLiteral(0))))));
    }

    private void AddInlineOptionalArrayOfOptionalFieldWrite(MethodReference method, string fieldName,
        TypeReference targetType,
        TypeReference primitiveType, IEnumerable<string> injectedArguments)
    {
        var field = new Variable(fieldName);
        var outerValue = new Variable($"__{fieldName}OuterValue");
        var value = new Variable($"__{fieldName}Value");
        var control = new Variable($"__{fieldName}Iter");
        method.If(new ExtractValueExpression(field, $"__{fieldName}OuterValue"), out var ifStatement);
        ifStatement.Add(new Call(_streamVar, "WriteByte", new IntegerLiteral(1)));
        ifStatement.Add(new Call(new CastExpression("VarInt", new GetField(outerValue, "Length")), "WriteTo",
            _streamVar, _stateVar));
        ifStatement.For(new VariableDeclarationStatement("int", $"__{fieldName}Iter", new IntegerLiteral(0)),
            new LessThan(control, new GetField(outerValue, "Length")), new PostIncrement(control),
            loop => loop.Add(
                new IfStatement(new ExtractValueExpression(new Subscript(outerValue, control), $"__{fieldName}Value"))
                    .Add(
                        new Call(_streamVar, "WriteByte", new IntegerLiteral(1))).Add(
                        new Call(targetType == primitiveType ? value : new CastExpression(primitiveType, value),
                            "WriteTo",
                            [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]))
                    .Else(els => els.Add(new Call(_streamVar, "WriteByte", new IntegerLiteral(0))))));
        ifStatement.Else(els => els.Add(new Call(_streamVar, "WriteByte", new IntegerLiteral(0))));
    }

    private void AddEnumFieldWrite(MethodReference method, string fieldName, TypeReference primitiveType)
    {
        var field = new Variable(fieldName);
        method.Add(new Call(new CastExpression(primitiveType, new CastExpression("int", field)), "WriteTo", _streamVar,
            _stateVar));
    }

    private void AddArrayFieldWrite(MethodReference method, string fieldName, TypeReference targetSubType,
        TypeReference primitiveSubType, string lengthControl, IEnumerable<string> injectedArguments)
    {
        var lengthVarName = $"__{fieldName}_length__";
        var iterVarName = $"__{fieldName}_iter__";
        method.DeclareVariable(typeof(int), lengthVarName, new InjectedExpression(lengthControl));
        var lengthVar = new Variable(lengthVarName);
        var iterVar = new Variable(iterVarName);
        var field = new Variable(fieldName);
        IExpression baseVar = new Subscript(field, iterVar);
        if (targetSubType != primitiveSubType)
        {
            baseVar = new CastExpression(primitiveSubType, baseVar);
        }

        var call = new Call(baseVar, "WriteTo",
            [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]);
        method.For(new VariableDeclarationStatement(iterVarName, new IntegerLiteral(0)),
            new LessThan(iterVar, lengthVar), new PostIncrement(iterVar),
            loop => loop.Add(call));
    }

    private void AddOptionalFieldWrite(MethodReference method, string fieldName, TypeReference targetType,
        TypeReference primitiveType, string optionalControl, IEnumerable<string> injectedArguments)
    {
        IExpression baseVar = new Variable(fieldName);
        if (targetType != primitiveType)
        {
            baseVar = new CastExpression(primitiveType, baseVar);
        }

        var call = new Call(baseVar, "WriteTo",
            [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]);
        method.If(new InjectedExpression(optionalControl), ifStatement => ifStatement.Add(call));
    }

    private void AddSplitEnumFieldWrite(MethodReference method, string fieldName) =>
        method.Add(new Call(new NullPropagate(new Variable(fieldName)), "Write", _streamVar, _stateVar));

    private void AddOptionalArrayFieldWrite(MethodReference method, string fieldName, TypeReference targetSubType,
        TypeReference primitiveSubType, string lengthControl, string optionalControl,
        IEnumerable<string> injectedArguments)
    {
        method.If(new InjectedExpression(optionalControl), block =>
        {
            var lengthVarName = $"__{fieldName}_length__";
            var iterVarName = $"__{fieldName}_iter__";
            block.DeclareVariable(typeof(int), lengthVarName, new InjectedExpression(lengthControl));
            var lengthVar = new Variable(lengthVarName);
            var iterVar = new Variable(iterVarName);
            var field = new Variable(fieldName);
            IExpression baseVar = new Subscript(field, iterVar);
            if (targetSubType != primitiveSubType)
            {
                baseVar = new CastExpression(primitiveSubType, baseVar);
            }

            var call = new Call(baseVar, "WriteTo",
                [_streamVar, _stateVar, ..injectedArguments.Select(x => new InjectedExpression(x))]);
            block.For(new VariableDeclarationStatement(iterVarName, new IntegerLiteral(0)),
                new LessThan(iterVar, lengthVar), new PostIncrement(iterVar),
                loop => loop.Add(call));
        });
    }
}