using Blocks.Net.Nbt;
using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct PartialDataComponentMatcher
{
    [PacketEnum(typeof(VarInt))] public StructuredComponent Type;
    [PacketField] public NbtTag Predicate;
}