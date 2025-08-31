using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots;

[SubPacket]
public partial struct Slot
{
    [PacketField] public VarInt ItemCount;
    [PacketOptionalField("ItemCount > 0")] public SlotData Data;
    
    // We need a nice way to build up a slot
}