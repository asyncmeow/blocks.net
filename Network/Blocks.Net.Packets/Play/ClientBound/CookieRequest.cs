using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("cookie_request",true,"Play")]
public partial class CookieRequest : IPacket
{
    [PacketField] public Identifier Cookie;
}