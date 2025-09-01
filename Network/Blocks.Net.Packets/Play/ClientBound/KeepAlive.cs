using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("keep_alive",true,"Play")]
public partial class ClientboundKeepAlive : IPacket
{
    [PacketField] public long KeepAliveId;
}