using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Advancements;

[SubPacket]
public partial struct Advancement
{
    [PacketField] public Identifier? ParentId;
    [PacketField] public AdvancementDisplay? DisplayData;
    [PacketField] public string[] NestedRequirements;
    [PacketField] public bool SendsTelemetryData;
}