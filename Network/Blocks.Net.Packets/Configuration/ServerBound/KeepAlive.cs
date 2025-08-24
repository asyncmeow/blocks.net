using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ServerBound;

[PublicAPI]
[Packet(0x04,false,"Configuration")]
public partial class KeepAlive : IPacket
{
    [PacketField] public long KeepAliveId;
}