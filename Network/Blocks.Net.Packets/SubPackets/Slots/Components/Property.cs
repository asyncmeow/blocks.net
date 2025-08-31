using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct Property
{
    [PacketField] public string Name;
    [PacketField] public bool ExactMatch;

    [PacketOptionalField(nameof(ExactMatch))]
    public string ExactValue;

    [PacketOptionalField("!ExactMatch")] public string MinValue;
    [PacketOptionalField("!ExactMatch")] public string MaxValue;
}