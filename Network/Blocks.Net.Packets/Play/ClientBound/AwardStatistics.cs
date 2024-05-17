using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x04,true,"Play")]
public partial class AwardStatistics : IPacket
{
    [PacketField] public VarInt Count;
    [PacketArrayField("Count")] public Statistic[] Statistics;
}