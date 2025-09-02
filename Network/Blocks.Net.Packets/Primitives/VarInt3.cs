using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Primitives;

[SubPacket]
public partial struct VarInt3
{
    [PacketField] public VarInt X;
    [PacketField] public VarInt Y;
    [PacketField] public VarInt Z;
}