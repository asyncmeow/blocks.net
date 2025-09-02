using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("select_trade",false,"Play")]
public partial class SelectTrade : IPacket
{
    [PacketField] public VarInt SelectedSlot;
}