using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("move_player_pos_rot",false,"Play")]
public partial class SetPlayerPositionAndRotation : IPacket
{
    [PacketField] public Double3 Position;
    [PacketField] public float Yaw;
    [PacketField] public float Pitch;
    [PacketField] public byte Flags;
}