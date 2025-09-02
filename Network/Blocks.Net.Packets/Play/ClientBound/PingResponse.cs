using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("pong_response",true,"Play")]
public partial class PingResponse : IPacket
{
    [PacketField] public long Payload;
}