using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("move_player_rot",false,"Play")]
public partial class SetPlayerRotation : IPacket
{
    [PacketField] public float Yaw;
    [PacketField] public float Pitch;
    [PacketField] public byte Flags;
}