using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet(0x03,true,"Configuration")]
public partial class KeepAlive : IPacket
{
    [PacketField] public long KeepAliveId;
}