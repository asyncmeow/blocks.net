using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Advancements;

[SubPacket]
public partial struct AdvancementProgress
{
    [PacketField] public Identifier Key;
    [PacketField] public AdvancementCriteria[] Criteria;
}