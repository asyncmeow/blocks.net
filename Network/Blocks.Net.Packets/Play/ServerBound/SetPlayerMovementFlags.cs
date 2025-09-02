using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("move_player_status_only",false,"Play")]
public partial class SetPlayerMovementFlags : IPacket
{
    [PacketField] public byte Flags;
}