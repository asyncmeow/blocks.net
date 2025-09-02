using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("container_close",false,"Play")]
public partial class CloseContainer : IPacket
{
    [PacketField] public VarInt WindowId;
}