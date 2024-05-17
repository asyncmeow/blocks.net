using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct Statistic
{
    [PacketField] public VarInt CategoryId;
    [PacketField] public VarInt StatisticId;
    [PacketField] public VarInt Value;
}