using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Primitives;

[SubPacket]
public partial struct GlobalPosition
{
    [PacketField] public Identifier Dimension;
    [PacketField] public Position Pos;
}