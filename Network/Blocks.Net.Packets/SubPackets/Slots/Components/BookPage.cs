using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct BookPage
{
    [PacketField] public string RawContent;
    [PacketField] public string? FilteredContent;
}