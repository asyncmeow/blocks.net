using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("projectile_power",true,"Play")]
public partial class ProjectilePower : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public double Power;
}