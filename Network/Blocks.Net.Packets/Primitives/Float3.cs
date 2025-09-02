using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Primitives;

[SubPacket]
public partial struct Float3
{
    [PacketField] public float X;
    [PacketField] public float Y;
    [PacketField] public float Z;
}