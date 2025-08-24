using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.SubPackets.Configuration;

[PublicAPI]
[SubPacket]
public partial struct ReportDetails
{
    [PacketField] public string Title;
    [PacketField] public string Description;
}