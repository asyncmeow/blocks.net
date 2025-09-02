using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots;

[SubPacket]
public partial struct HashedSlot
{
    [PacketField] public bool HasItem;
    [PacketField] public VarInt ItemId;
    [PacketField] public VarInt ItemCount;
    [PacketField] public HashedComponent[] ComponentsToAdd;
    [PacketField] public RemovedComponent[] ComponentsToRemove;
}