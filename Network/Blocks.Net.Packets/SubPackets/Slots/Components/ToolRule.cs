using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct ToolRule
{
    [PacketField] public IdSet BlockSet;
    [PacketField] public float? Speed;
    [PacketField] public bool? CorrectDropForBlocks;
}