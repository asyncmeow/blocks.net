using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct PositionVector
{
    [PacketField] public double X;
    [PacketField] public double Y;
    [PacketField] public double Z;
}