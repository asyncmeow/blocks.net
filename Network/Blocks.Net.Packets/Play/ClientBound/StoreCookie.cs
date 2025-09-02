using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("store_cookie",true,"Play")]
public partial class StoreCookie : IPacket
{
    [PacketField] public Identifier Key;
    [PacketField] public byte[] Payload;
}