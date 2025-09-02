using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.Slots;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_player_inventory",true,"Play")]
public partial class SetPlayerInventorySlot : IPacket
{
    [PacketField] public VarInt SlotIndex;
    [PacketField] public Slot SlotData;
}