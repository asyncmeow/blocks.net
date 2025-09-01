using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.Slots;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("container_set_slot", true, "Play")]
public partial class SetContainerSlot : IPacket
{
    [PacketField] public VarInt WindowId;
    [PacketField] public VarInt StateId;
    [PacketField] public short SlotIndex;
    [PacketField] public Slot SlotData;
}