using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Status.ServerBound;

[PublicAPI]
[Packet(0x01,false,"Status")]
public partial class PingRequest : IPacket
{
    [PacketField] public long Payload;
}