using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Primitives;

[SubPacket]
public partial struct Double3
{
    [PacketField] public double X;
    [PacketField] public double Y;
    [PacketField] public double Z;
}