using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct VelocityVector
{
    [PacketField] public short X;
    [PacketField] public short Y;
    [PacketField] public short Z;
}