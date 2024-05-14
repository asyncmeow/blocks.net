using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Status.ClientBound;

[PublicAPI]
[Packet(0x01,true,"Status")]
public partial class PingResponse : IPacket
{
    [PacketField] public long Payload;
}