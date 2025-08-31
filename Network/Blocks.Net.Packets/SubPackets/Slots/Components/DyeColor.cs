using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct DyeColor
{
    [PacketEnum(typeof(VarInt))] public Color Color;
}