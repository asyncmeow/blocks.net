using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct DamageReduction
{
    [PacketField] public float HorizontalBlockingAngle;
    [PacketField] public IdSet? Type;
    [PacketField] public float Base;
    [PacketField] public float Factor;
}