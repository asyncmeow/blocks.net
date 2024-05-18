using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct ExplosionRecord
{
    [PacketField] public sbyte X;
    [PacketField] public sbyte Y;
    [PacketField] public sbyte Z;
}