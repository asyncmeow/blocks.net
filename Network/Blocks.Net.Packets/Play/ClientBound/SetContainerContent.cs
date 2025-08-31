using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.Slots;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x12, true, "Play")]
public partial class SetContainerContent : IPacket
{
    [PacketField] public VarInt WindowId;
    [PacketField] public VarInt StateId;
    [PacketField] public Slot[] SlotData;
    [PacketField] public Slot CarriedItem;
}