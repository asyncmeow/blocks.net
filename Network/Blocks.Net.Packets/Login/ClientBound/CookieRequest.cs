using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ClientBound;

[PublicAPI]
[Packet(0x05,true, "Login")]
public partial class CookieRequest : IPacket
{
    [PacketField] public Identifier Key;
}