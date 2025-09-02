using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("player_rotation",true,"Play")]
public partial class PlayerRotation : IPacket
{
    [PacketField] public float Yaw;
    [PacketField] public float Pitch;
}