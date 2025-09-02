using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("ping_request",false,"Play")]
public partial class PingRequest : IPacket
{
    [PacketField] public long Payload;
}