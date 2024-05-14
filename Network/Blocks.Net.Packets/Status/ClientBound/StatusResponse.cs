using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Status.ClientBound;

[PublicAPI]
[Packet(0x00,true,"Status")]
public partial class StatusResponse : IPacket
{
    [PacketField] public string JsonResponse;
}