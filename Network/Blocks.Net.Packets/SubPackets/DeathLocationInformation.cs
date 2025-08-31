using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct DeathLocationInformation
{
    [PacketField] public Identifier Dimension;
    [PacketField] public Identifier Location;
}