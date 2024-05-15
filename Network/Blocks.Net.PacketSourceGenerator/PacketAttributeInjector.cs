using Microsoft.CodeAnalysis;

namespace Blocks.Net.PacketSourceGenerator;

[Generator]
public partial class PacketAttributeInjector : ISourceGenerator
{
    [EmbedMe("Packet.cs")] public static string PacketAttribute;
    [EmbedMe("PacketArrayField.cs")] public static string PacketArrayFieldAttribute;
    [EmbedMe("PacketEnum.cs")] public static string PacketEnum;
    [EmbedMe("PacketField.cs")] public static string PacketField;
    [EmbedMe("PacketOptionalField.cs")] public static string PacketOptionalField;
    [EmbedMe("SubPacket.cs")] public static string SubPacket;
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        InjectAllEmbeddedSourceFiles(context);
    }
}