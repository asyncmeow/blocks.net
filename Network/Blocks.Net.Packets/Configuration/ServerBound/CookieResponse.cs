using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;
using Byte = Blocks.Net.Packets.Primitives.Byte;

namespace Blocks.Net.Packets.Configuration.ServerBound;

[PublicAPI]
[Packet("cookie_response",false,"Configuration")]
public partial class CookieResponse : IPacket
{
    [PacketField] public Identifier Key;
    [PacketField] public byte[]? Payload;
}