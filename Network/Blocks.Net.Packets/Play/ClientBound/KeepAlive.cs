using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x24,true,"Play")]
public partial class KeepAlive : IPacket
{
    [PacketField] public long KeepAliveId;
}