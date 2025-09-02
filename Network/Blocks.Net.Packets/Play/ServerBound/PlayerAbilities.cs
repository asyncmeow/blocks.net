using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("player_abilities",false,"Play")]
public partial class PlayerAbilities : IPacket
{
    [PacketField] public byte IsFlying;
}