using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct BannerLayer
{
    [PacketField] public IdOrBannerPattern PatternType;
    [PacketField] public DyeColor Color;
}