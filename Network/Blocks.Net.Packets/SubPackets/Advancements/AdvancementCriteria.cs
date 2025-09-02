using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Advancements;

[SubPacket]
public partial struct AdvancementCriteria
{
    [PacketField] public Identifier CriterionIdentifier;
    [PacketField] public long? DateOfAchieving;
}