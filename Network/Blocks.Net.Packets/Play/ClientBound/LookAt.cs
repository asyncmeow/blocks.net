using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("player_look_at", true, "Play")]
public partial class LookAt : IPacket
{

    [PacketEnum(typeof(VarInt))] public AimingPositions AimingPosition;
    [PacketField] public Double3 Target;
    [PacketField] public LookAtEntity? Entity;

}