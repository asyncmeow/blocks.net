using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x34,true,"Play")]
public partial class PingResponse : IPacket
{
    [PacketField] public long Payload;
}