using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;


[SubPacket]
public partial struct FireworkExplosion
{
    public enum ExplosionShape
    {
        SmallBall,
        LargeBall,
        Star,
        Creeper,
        Burst
    }

    [PacketField] public ExplosionShape Shape;
    [PacketField] public int[] Colors;
    [PacketField] public int[] FadeColors;
    [PacketField] public bool HasTrail;
    [PacketField] public bool HasTwinkle;
}