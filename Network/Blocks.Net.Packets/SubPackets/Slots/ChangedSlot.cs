using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots;

[SubPacket]
public partial struct ChangedSlot
{
    [PacketField] public short SlotNumber;
    [PacketField] public HashedSlot SlotData;
}