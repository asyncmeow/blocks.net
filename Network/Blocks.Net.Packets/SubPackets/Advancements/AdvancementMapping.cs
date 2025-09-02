using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Advancements;

[SubPacket]
public partial struct AdvancementMapping
{
    [PacketField] public Identifier Key;
    [PacketField] public Advancement Value;
}