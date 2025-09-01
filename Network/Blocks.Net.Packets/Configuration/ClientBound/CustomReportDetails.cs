using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;
using ReportDetails = Blocks.Net.Packets.SubPackets.Configuration.ReportDetails;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet("custom_report_details", true, "Configuration")]
public partial class CustomReportDetails : IPacket
{
    [PacketField] public ReportDetails[] Details;
}