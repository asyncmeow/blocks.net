using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x03, true, "Play")]
public partial class AwardStatistics
{
    [PacketField] public Statistic[] Statistics;
}