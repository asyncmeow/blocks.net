using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;
using Byte = Blocks.Net.Packets.Primitives.Byte;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet(0x0A, true, "Configuration")]
public partial class StoreCookie : IPacket
{
    [PacketField] public Identifier Key;
    [PacketField] public byte[] Payload;
}