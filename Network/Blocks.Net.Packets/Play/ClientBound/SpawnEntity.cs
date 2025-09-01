using System.ComponentModel.DataAnnotations;
using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Double3 = Blocks.Net.Packets.Primitives.Double3;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("add_entity",true,"Play")]
public partial class SpawnEntity : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public Uuid Uuid;
    [PacketField] public RegistryReference Type;
    [PacketField] public Double3 Position;
    [PacketField] public Angle Pitch;
    [PacketField] public Angle Yaw;
    [PacketField] public Angle HeadYaw;
    [PacketField] public VarInt Data;
    [PacketField] public VelocityVector Velocity;
}