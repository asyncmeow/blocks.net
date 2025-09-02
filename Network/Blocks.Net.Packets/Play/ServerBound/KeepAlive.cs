using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("keep_alive",false,"Play")]
public partial class KeepAlive : IPacket
{
    [PacketField] public long KeepAliveId;
}