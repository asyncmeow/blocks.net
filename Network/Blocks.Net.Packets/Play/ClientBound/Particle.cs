using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("level_particles", true, "Play")]
public partial class Particle
{
    [PacketField] public bool LongDistance;
    [PacketField] public bool AlwaysVisible;
    [PacketField] public Double3 Position;
    [PacketField] public Float3 Offset;
    [PacketField] public float MaxSpeed;
    [PacketField] public int ParticleCount;
    [PacketField] public ParticleImpl ParticleToSpawn;
}