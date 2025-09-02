using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("cookie_response",false,"Play")]
public partial class CookieResponse : IPacket
{
    [PacketField] public Identifier Key;
    [PacketField] public byte[] Payload;
}