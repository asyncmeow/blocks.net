using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x1E,true,"Play")]
public partial class Explosion : IPacket
{
    public enum Interaction
    {
        Keep = 0,
        Destroy = 1,
        DestroyWithDecay = 2,
        TriggerBlock = 3
    }
    
    [PacketField] public double X;
    [PacketField] public double Y;
    [PacketField] public double Z;
    [PacketField] public float Strength;
    [PacketField] public VarInt RecordCount;
    [PacketArrayField("RecordCount")] public ExplosionRecord[] Records;
    [PacketField] public float PlayerMotionX;
    [PacketField] public float PlayerMotionY;
    [PacketField] public float PlayerMotionZ;
    [PacketEnum(typeof(VarInt))] public Interaction BlockInteraction;
    [PacketField] public ParticleImpl SmallExplosionParticle;
    [PacketField] public ParticleImpl LargeExplosionParticle;
    [PacketField] public SoundEffect SoundEffect;
}

