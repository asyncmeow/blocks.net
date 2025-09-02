using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("player_position", true, "Play")]
public partial class SynchronizePlayerPosition : IPacket
{
    [PacketField] public VarInt TeleportId;
    [PacketField] public Double3 Position;
    [PacketField] public Double3 Velocity;
    [PacketField] public float Yaw;
    [PacketField] public float Pitch;
    [PacketEnum(typeof(int))] public TeleportFlags TeleportFlags;
}