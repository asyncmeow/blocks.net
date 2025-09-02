using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct MinecartStep
{
    [PacketField] public Double3 Postition;
    [PacketField] public Double3 Velocity;
    [PacketField] public Angle Yaw;
    [PacketField] public Angle Pitch;
    [PacketField] public float Weight;
}