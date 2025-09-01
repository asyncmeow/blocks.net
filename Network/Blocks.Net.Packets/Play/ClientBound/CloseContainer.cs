using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("container_close",true,"Play")]
public partial class CloseContainer : IPacket
{
    [PacketField] public VarInt WindowId;
}