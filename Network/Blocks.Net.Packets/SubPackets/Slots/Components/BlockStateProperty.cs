using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct BlockStateProperty
{
    [PacketField] public string Name;
    [PacketField] public string Value;
}