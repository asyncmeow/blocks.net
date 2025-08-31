using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots;

[SubPacket]
public partial class SlotData
{
    [PacketField] public ItemId ItemId;
    [PacketField] public VarInt NumberToAdd;
    [PacketField] public VarInt NumberToRemove;
    [PacketArrayField(nameof(NumberToAdd))] public StructuredComponentImpl[] ComponentsToAdd;
    [PacketArrayField(nameof(NumberToRemove))] public RemovedComponent[] ComponentsToRemove;
}