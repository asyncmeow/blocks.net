using Blocks.Net.Packets.SubPackets.Slots;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_cursor_item",true,"Play")]
public partial class SetCursorItem : IPacket
{
    [PacketField] public Slot CarriedItem;
}