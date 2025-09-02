using Blocks.Net.Packets.SubPackets.Slots;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("set_creative_mode_slot",false,"Play")]
public partial class SetCreativeModeSlot : IPacket
{
    [PacketField] public short SlotIndex;
    [PacketField] public Slot ClickedItem;
}