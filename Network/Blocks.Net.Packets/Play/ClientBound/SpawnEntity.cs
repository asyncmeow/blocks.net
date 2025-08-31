using System.ComponentModel.DataAnnotations;
using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x01,true,"Play")]
public partial class SpawnEntity : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public Uuid Uuid;
    [PacketField] public RegistryReference Type;
    [PacketField] public PositionVector Position;
    [PacketField] public Angle Pitch;
    [PacketField] public Angle Yaw;
    [PacketField] public Angle HeadYaw;
    [PacketField] public VarInt Data;
    [PacketField] public VelocityVector Velocity;
}