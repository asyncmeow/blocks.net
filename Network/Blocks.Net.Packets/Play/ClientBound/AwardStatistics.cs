using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("award_stats", true, "Play")]
public partial class AwardStatistics
{
    [PacketField] public Statistic[] Statistics;
}