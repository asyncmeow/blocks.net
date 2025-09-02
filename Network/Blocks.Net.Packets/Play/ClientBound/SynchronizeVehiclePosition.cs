using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("teleport_entity",true,"Play")]
public partial class SynchronizeVehiclePosition : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public Double3 Position;
    [PacketField] public Double3 Velocity;
    [PacketField] public float Yaw;
    [PacketField] public float Pitch;
    [PacketEnum(typeof(int))] public TeleportFlags Flags;
    [PacketField] public bool OnGround;
}