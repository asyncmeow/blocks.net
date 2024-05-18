using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x27,true,"Play")]
public partial class Particle : IPacket
{
    [PacketEnum(typeof(VarInt))]  public Enums.Particle ParticleId;
    [PacketField] public bool LongDistance;
    [PacketField] public double X;
    [PacketField] public double Y;
    [PacketField] public double Z;
    [PacketField] public double OffsetX;
    [PacketField] public double OffsetY;
    [PacketField] public double OffsetZ;
    [PacketField] public float MaxSpeed;
    [PacketField] public int ParticleCount;
    [PacketSplitEnumDataField("ParticleId")] public IParticle Data;
}