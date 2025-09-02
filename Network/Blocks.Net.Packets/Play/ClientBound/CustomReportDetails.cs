using Blocks.Net.Packets.SubPackets.Configuration;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("custom_report_details",true,"Play")]
public partial class CustomReportDetails : IPacket
{
    [PacketField] public ReportDetails[] Details;
}