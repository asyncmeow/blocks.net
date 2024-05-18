using Blocks.Net.Nbt;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct SuggestionMatch
{
    [PacketField] public string Match;
    [PacketField] public bool HasTooltip;
    [PacketOptionalField("HasTooltip")] public NbtTag Tooltip;
}