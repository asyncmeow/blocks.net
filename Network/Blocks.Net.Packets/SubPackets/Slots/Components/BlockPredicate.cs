using Blocks.Net.Nbt;
using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct BlockPredicate
{
    [PacketField] public IdSet? BlockSet;
    [PacketField] public Property[]? Properties;
    [PacketField] public NbtTag? Nbt;
    [PacketField] public StructuredComponentImpl[] DataComponents;
    [PacketField] public PartialDataComponentMatcher[] PartialDataComponentPredicates;
}