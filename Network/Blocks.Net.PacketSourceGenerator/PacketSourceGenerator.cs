using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Blocks.Net.PacketSourceGenerator;

[Generator]
public class PacketSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    private static readonly Dictionary<string, string> PrimitiveRemap = new()
    {
        {"bool","Blocks.Net.Packets.Primitives.Boolean"},
        {"byte","UnsignedByte"},
        {"sbyte","Blocks.Net.Packets.Primitives.Byte"},
        {"short","Short"},
        {"ushort","UnsignedShort"},
        {"int","Int"},
        {"long","Long"},
        {"float","Float"},
        {"double","Blocks.Net.Packets.Primitives.Double"},
        {"string","Blocks.Net.Packets.Primitives.String"},
        {"NbtTag","Blocks.Net.Packets.Primitives.Nbt"},
        {"Guid","Blocks.Net.Packets.Primitives.Uuid"},
        {"Uuid","Blocks.Net.Packets.Primitives.Uuid"},
    };    
    public void Execute(GeneratorExecutionContext context)
    {

        // context.AddSource("IPacket.g.cs",@"
        //     namespace Blocks.Net.Packets;
        //     partial class 
        // ");
        // foreach (var syntaxTree in context.Compilation.SyntaxTrees)
        // {
        // }
        
        
        // All of these are defined in the Attributes folder, just doing it here
        context.AddSource("Packet.g.cs",
            "namespace Blocks.Net.PacketSourceGenerator.Attributes;\n\n[AttributeUsage(AttributeTargets.Class)]\npublic class Packet(int id, bool clientBound = true, string state = \"Play\") : Attribute;");
        context.AddSource("PacketArrayField.g.cs",
            "namespace Blocks.Net.PacketSourceGenerator.Attributes;\n/// <summary>\n/// Used on arrays of primitive types, or structs, or fielded enums.\n/// </summary>\n/// <param name=\"arraySizeControl\">An expression resulting in a value convertible to an int for the size of the array to read</param>\n[AttributeUsage(AttributeTargets.Field)]\npublic class PacketArrayField(string arraySizeControl) : Attribute;");
        context.AddSource("PacketField.g.cs",
            "namespace Blocks.Net.PacketSourceGenerator.Attributes;\n\n/// <summary>\n/// Marks this field as a field for a packet\n/// The type of the field must be either in the Blocks.Net.Packets.Primitives namespace or have a static ReadFrom(MemoryStream) method\n/// And a static WriteTo(MemoryStream) method.\n///\n/// If the type of the field is an enum it is assumed to be a Primitives.VarInt for parsing purposes\n///\n/// Other conversions are as follows for parsing classes that are implicitly converted to the field and reverse\n/// bool   ->  Primitives.Boolean\n/// byte   ->  Primitives.UnsignedByte\n/// sbyte  ->  Primitives.Byte\n/// short  ->  Primitives.Short\n/// ushort ->  Primitives.UnsignedShort\n/// int    ->  Primitives.Int\n/// long   ->  Primitives.Long\n/// float  ->  Primitives.Float\n/// double ->  Primitives.Double\n/// string ->  Primitives.String\n///\n/// </summary>\n[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]\npublic class PacketField : Attribute;");
        context.AddSource("PacketOptionalField.g.cs",
            "namespace Blocks.Net.PacketSourceGenerator.Attributes;\n\n/// <summary>\n/// Sets this field as an optional field\n/// </summary>\n/// <param name=\"controllingCondition\">An expression that results in a bool that determines if this optional is included, can use previously parsed fields</param>\npublic class PacketOptionalField(string controllingCondition) : Attribute;");
        context.AddSource("SubPacket.g.cs",
            "namespace Blocks.Net.PacketSourceGenerator.Attributes;\n\n/// <summary>\n/// Generate .ReadFrom and .WriteTo methods for this class like it were some form of sub packet\n/// (Mostly used for generating new primitive types based on structured data)\n/// </summary>\n[AttributeUsage(AttributeTargets.Struct)]\npublic class SubPacket : Attribute;");
        context.AddSource("PacketEnum.g.cs","namespace Blocks.Net.PacketSourceGenerator.Attributes;\n\n/// <summary>\n/// Used on enum fields to determine the actual packet primitive type of the field\n/// </summary>\n/// <param name=\"EnumType\">The primitive type of the enum</param>\n[AttributeUsage(AttributeTargets.Field)]\npublic class PacketEnum(Type EnumType) : Attribute;");
        
        
        Dictionary<string, Dictionary<int, string>> serverBoundPackets = [];
        var foundPacketParser = false;


        foreach (var tree in context.Compilation.SyntaxTrees)
        {
            var semanticModel = context.Compilation.GetSemanticModel(tree);
            var ns = tree.GetRoot().DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();
            var usingDecls = tree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
            var header = string.Join("\n", usingDecls.Select(x => x.ToString()));
            var namespaceName = ns?.Name.ToString() ?? "";
            Console.WriteLine($"Found namespace: {namespaceName}");

            var interfaces = tree.GetRoot().DescendantNodes().OfType<InterfaceDeclarationSyntax>();
            var classes = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
            var structs = tree.GetRoot().DescendantNodes().OfType<StructDeclarationSyntax>();

            foreach (var clazz in classes)
            {
                var className = clazz.Identifier.Text;
                Console.WriteLine($"Found class {clazz.Identifier.Text}");
                var attrs = clazz.DescendantNodes().OfType<AttributeSyntax>().ToArray();
                if ($"{namespaceName}.{className}" == "Blocks.Net.Packets.PacketParser") foundPacketParser = true;
                if (attrs.FirstOrDefault(
                        a => a.DescendantTokens().Any(dt => CheckForPacketAttribute(dt, semanticModel))) is { } attr)
                {
                    Console.WriteLine($"Found attr: {attr}");
                    var descendentTokens = attr.DescendantTokens().ToArray();
                    var byteNode = descendentTokens.FirstOrDefault(x => x.IsKind(SyntaxKind.NumericLiteralToken));
                    if (byteNode == null) continue;
                    var packetId = (int)(byteNode.Value ?? 0xff);
                    var sb = StartPacketClass(namespaceName, className, packetId,header);
                    var clientBoundToken = descendentTokens.FirstOrDefault(x =>
                        x.IsKind(SyntaxKind.TrueKeyword) || x.IsKind(SyntaxKind.FalseKeyword));
                    var clientBound = clientBoundToken == null || clientBoundToken.IsKind(SyntaxKind.TrueKeyword);
                    var state =
                        descendentTokens.FirstOrDefault(x => x.IsKind(SyntaxKind.StringLiteralToken)) is var syntaxToken
                            ? (string)syntaxToken.Value!
                            : "Play";
                    // Generate the needed methods
                    var readFromSb = StartReadFrom(className);
                    var writeSb = StartWrite();
                    List<string> fields = [];
                    // Now let's go over every field
                    foreach (var field in clazz.DescendantNodes().OfType<FieldDeclarationSyntax>())
                    {
                        
                        GeneratePacketFieldImpl(field, semanticModel, readFromSb, writeSb, fields);
                    }
                    
                    EndReadFrom(sb, readFromSb, fields.ToArray());
                    EndWrite(sb, writeSb);
                    
                    if (!clientBound)
                    {
                        var dict = serverBoundPackets.TryGetValue(state, out var d)
                            ? d
                            : serverBoundPackets[state] = [];
                        dict[packetId] = $"{namespaceName}.{className}";
                    }
                    context.AddSource($"{namespaceName}.{className}.g.cs", StopPacketClass(sb));
                }
            }

            foreach (var structure in structs)
            {
                var structName = structure.Identifier.Text;
                Console.WriteLine($"Found struct {structName}");
                var attrs = structure.DescendantNodes().OfType<AttributeSyntax>().ToArray();
                if (attrs.FirstOrDefault(a =>
                        a.DescendantTokens().Any(dt => CheckForSubPacketAttribute(dt, semanticModel))) is { } attr)
                {
                    var sb = StartSubPacketStruct(namespaceName, structName,header);

                    var readFromSb = StartReadFrom(structName,true);
                    var writeSb = StartWrite(true);
                    List<string> fields = [];
                    // Now let's go over every field
                    foreach (var field in structure.DescendantNodes().OfType<FieldDeclarationSyntax>())
                    {
                        GeneratePacketFieldImpl(field, semanticModel, readFromSb, writeSb, fields);
                    }
                    
                    EndReadFrom(sb, readFromSb, fields.ToArray());
                    EndWrite(sb, writeSb);

                    context.AddSource($"{namespaceName}.{structName}.g.cs", StopPacketClass(sb));
                }
            }
        }

        Console.WriteLine($"Found packet parser: {foundPacketParser}");

        if (foundPacketParser)
        {
            // Now we need to generate our own packet parser class with all the delegates
            var sb = new StringBuilder();
            sb.Append("namespace Blocks.Net.Packets;\nstatic partial class PacketParser {\n");
            foreach (var kvp in serverBoundPackets)
            {
                var state = kvp.Key;
                var delegates = kvp.Value;
                // Now lets create the dictionary
                sb.Append(
                    $"    public static Dictionary<int,Func<MemoryStream,IPacket>> {state}ServerBoundPackets = new()\n    {{\n");
                foreach (var kvp2 in delegates)
                {
                    sb.Append($"        {{0x{kvp2.Key:X},{kvp2.Value}.ReadFrom}},\n");
                }

                sb.Append("    };\n");
            }

            sb.Append("}\n");
            context.AddSource("PacketParser.g.cs", sb.ToString());
        }
    }

    private void GeneratePacketFieldImpl(FieldDeclarationSyntax field, SemanticModel semanticModel,
        StringBuilder readFromMethodStringBuffer, StringBuilder writeMethodStringBuffer, List<string> fields)
    {
        Console.WriteLine($"Found field: {field}");
        // Now let's check first if the field has a very simple attribute
        var fieldAttrs = field.DescendantNodes().OfType<AttributeSyntax>().ToArray();
        Console.WriteLine(string.Join(", ", fieldAttrs.Select(x => x.ToString())));
        var decl = field.Declaration;
        var targetType = decl.Type.GetText().ToString().Trim();
        var name = decl.Variables.First().Identifier.Text;
        // The simplest type of packet field to handle
        if (fieldAttrs.FirstOrDefault(a => a.DescendantTokens().Any(dt => CheckForPacketFieldAttribute(dt, semanticModel))) is {} fieldAttr)
        {
            var primitiveType = PrimitiveRemap.TryGetValue(targetType, out var primType)
                ? primType
                : targetType;
            AddSimpleFieldRead(readFromMethodStringBuffer, name, targetType, primitiveType);
            AddSimpleFieldWrite(writeMethodStringBuffer, name, targetType, primitiveType);
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
            AddEnumFieldRead(readFromMethodStringBuffer, name, targetType, primitiveType);
            AddEnumFieldWrite(writeMethodStringBuffer, name, targetType, primitiveType);
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
            AddArrayFieldRead(readFromMethodStringBuffer, name, targetTypeSubType, primitiveSubType, stringValue);
            AddArrayFieldWrite(writeMethodStringBuffer, name, targetTypeSubType, primitiveSubType, stringValue);
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
            AddOptionalFieldRead(readFromMethodStringBuffer, name, targetType, primitiveType,stringValue);
            AddOptionalFieldWrite(writeMethodStringBuffer, name, targetType, primitiveType,stringValue);
            fields.Add(name);
        }
    }

    private bool CheckForPacketAttribute(SyntaxToken token, SemanticModel semanticModel)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        // var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        // Console.WriteLine($"Found attribute of type: {name}");
        return name == "Packet";
    }
    
    private bool CheckForSubPacketAttribute(SyntaxToken token, SemanticModel semanticModel)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        // var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        // Console.WriteLine($"Found attribute of type: {name}");
        return name == "SubPacket";
    }
    
    private bool CheckForPacketFieldAttribute(SyntaxToken token, SemanticModel semanticModel)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        // var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        // Console.WriteLine($"Found field attribute of type: {name}");
        return name == "PacketField";
    }

    private bool CheckForPacketEnumAttribute(SyntaxToken token, SemanticModel semanticModel)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        // var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        // Console.WriteLine($"Found field attribute of type: {name}");
        return name == "PacketEnum";
    }
    
    private bool CheckForPacketArrayFieldAttribute(SyntaxToken token, SemanticModel semanticModel)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        // var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        // Console.WriteLine($"Found field attribute of type: {name}");
        return name == "PacketArrayField";
    }

    private bool CheckForPacketOptionalFieldAttribute(SyntaxToken token, SemanticModel semanticModel)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        // var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        // Console.WriteLine($"Found field attribute of type: {name}");
        return name == "PacketOptionalField";
    }

    private StringBuilder StartPacketClass(string ns, string className, int id, string header)
    {
        var namespaceName = ns;
        StringBuilder sb = new();
        // Just make sure that we are using this here! As it's important for parsing purposes
        sb.Append("using Blocks.Net.Packets.Primitives;\n");
        sb.Append(header.Replace("using Blocks.Net.Packets.Primitives;","")).Append('\n');
        if (!string.IsNullOrEmpty(ns)) sb.Append($"namespace {namespaceName};\n");

        sb.Append($"partial class {className} {{\n");
        sb.Append($"    public int PacketId => 0x{id:X};\n");

        return sb;
    }

    private StringBuilder StartSubPacketStruct(string ns, string structName,string header)
    {
        var namespaceName = ns;
        StringBuilder sb = new();
        // Just make sure that we are using this here! As it's important for parsing purposes
        sb.Append("using Blocks.Net.Packets.Primitives;\n");
        sb.Append(header.Replace("using Blocks.Net.Packets.Primitives;","")).Append('\n');
        if (!string.IsNullOrEmpty(ns)) sb.Append($"namespace {namespaceName};\n");

        sb.Append($"partial struct {structName} {{\n");
        return sb;
    }

    private StringBuilder StartReadFrom(string className, bool isSubPacket = false)
    {
        var sb = new StringBuilder();
        sb.Append( "    /// <summary>\n");
        sb.Append($"    /// Reads a {className} {(isSubPacket ? "sub" : "")}packet from a memory stream\n");
        sb.Append( "    /// </summary>\n");
        sb.Append(
            $"    /// <param name=\"stream\">The {(isSubPacket ? "sub" : "")}packet stream{(isSubPacket ? "" : ", the length and id must already be consumed")}</param>\n");
        sb.Append($"    /// <returns>A {(isSubPacket ? "sub" : "")}packet constructed from the given stream</returns>\n");
        sb.Append($"    public static {className} ReadFrom(MemoryStream stream) {{\n");
        return sb;
    }

    private void AddSimpleFieldRead(StringBuilder sb, string fieldName, string targetType, string primitiveType) =>
        sb.Append(targetType == primitiveType
            ? $"        var {fieldName} = {targetType}.ReadFrom(stream);\n"
            : $"        var {fieldName} = ({targetType}){primitiveType}.ReadFrom(stream);\n");

    private void AddEnumFieldRead(StringBuilder sb, string fieldName, string enumType, string primitiveType) =>
        sb.Append($"        var {fieldName} = ({enumType})({primitiveType}.ReadFrom(stream).Value);\n");

    private void AddArrayFieldRead(StringBuilder sb, string fieldName, string targetSubType, string primitiveSubType,
        string lengthControl)
    {
        var lengthVarName = $"__{fieldName}_length__";
        var iterVarName = $"__{fieldName}_iter__";
        sb.Append($"        int {lengthVarName} = {lengthControl};\n");
        sb.Append($"        var {fieldName} = new {targetSubType}[{lengthVarName}];\n");
        sb.Append($"        for (var {iterVarName} = 0; {iterVarName} < {lengthVarName}; {iterVarName}++) {{\n");
        sb.Append(
            $"            {fieldName}[{iterVarName}] = {(targetSubType == primitiveSubType ? $"{primitiveSubType}.ReadFrom(stream)" : $"({targetSubType}){primitiveSubType}.ReadFrom(stream)")};\n");
        sb.Append($"        }}\n");
    }

    private void AddOptionalFieldRead(StringBuilder sb, string fieldName, string targetType, string primitiveType, string optionalControl)
    {
        sb.Append($"        {targetType} {fieldName} = default({targetType});\n");
        sb.Append($"        if ({optionalControl}) {{\n");
        sb.Append(targetType == primitiveType
            ? $"            {fieldName} = {targetType}.ReadFrom(stream);\n"
            : $"            {fieldName} = ({targetType}){primitiveType}.ReadFrom(stream);\n");
        sb.Append($"        }}\n");
    }
    
    private void EndReadFrom(StringBuilder classBuilder, StringBuilder sb, string[] fieldNames)
    {
        sb.Append("        return new() {\n");
        foreach (var field in fieldNames)
        {
            sb.Append($"           {field} = {field},\n");
        }
        sb.Append("        };\n");
        sb.Append("    }\n");
        classBuilder.Append(sb);
    }

    private StringBuilder StartWrite(bool isSubPacket=false)
    {
        var sb = new StringBuilder();
        sb.Append("    /// <inheritdoc />\n");
        if (!isSubPacket)
        {
            sb.Append("    public void Write(MemoryStream stream) {\n");
        }
        else
        {
            sb.Append("    public void WriteTo(MemoryStream stream) {\n");
        }
        return sb;
    }

    private void AddSimpleFieldWrite(StringBuilder sb, string fieldName, string targetType, string primitiveType)
    {
        sb.Append(targetType == primitiveType
            ? $"        {fieldName}.WriteTo(stream);\n"
            : $"        (({primitiveType}){fieldName}).WriteTo(stream);\n");
    }

    private void AddEnumFieldWrite(StringBuilder sb, string fieldName, string enumType, string primitiveType) =>
        sb.Append($"        (({primitiveType})(int){fieldName}).WriteTo(stream);\n");

    private void AddArrayFieldWrite(StringBuilder sb, string fieldName, string targetSubType, string primitiveSubType,
        string lengthControl)
    {
        var lengthVarName = $"__{fieldName}_length__";
        var iterVarName = $"__{fieldName}_iter__";
        sb.Append($"        int {lengthVarName} = {lengthControl};\n");
        sb.Append($"        for (var {iterVarName} = 0; {iterVarName} < {lengthVarName}; {iterVarName}++) {{\n");
        sb.Append($"            {(targetSubType == primitiveSubType ? $"{fieldName}[{iterVarName}]" : $"(({primitiveSubType}){fieldName}[{iterVarName}])")}.WriteTo(stream);\n");
        sb.Append("        }\n");
    }

    private void AddOptionalFieldWrite(StringBuilder sb, string fieldName, string targetType, string primitiveType,
        string optionalControl)
    {
        sb.Append($"        if ({optionalControl}) {{\n");
        sb.Append(targetType == primitiveType
            ? $"            {fieldName}.WriteTo(stream);\n"
            : $"            (({primitiveType}){fieldName}).WriteTo(stream);\n");
        sb.Append("        }\n");
    }
    
    private void EndWrite(StringBuilder classBuilder, StringBuilder sb)
    {
        sb.Append("    }\n");
        classBuilder.Append(sb);
    }

    private string StopPacketClass(StringBuilder sb)
    {
        sb.Append("}\n");
        return sb.ToString();
    }
}