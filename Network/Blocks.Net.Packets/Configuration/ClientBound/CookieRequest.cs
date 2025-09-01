using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[Packet("cookie_request",true,"Configuration")]
public partial class CookieRequest : IPacket
{
    [PacketField] public Identifier Key;
}