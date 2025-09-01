using Blocks.Net.Packets.Enums;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("explode",true,"Play")]
public partial class Explosion : IPacket
{
    [PacketField] public Double3 Position;
    [PacketField] public Double3? PlayerDeltaVelocity;
    [PacketField] public ParticleImpl ExplosionParticle;
    [PacketField] public IdOrSoundEvent ExplosionSound;
}