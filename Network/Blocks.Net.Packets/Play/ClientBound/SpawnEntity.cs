using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x01,true,"Play")]
public partial class SpawnEntity : IPacket
{
    [PacketField] public VarInt Id;
    [PacketField] public Guid EntityUuid;
    [PacketField] public VarInt Type;
    [PacketField] public double X;
    [PacketField] public double Y;
    [PacketField] public double Z;
    [PacketField] public Angle Pitch;
    [PacketField] public Angle Yaw;
    [PacketField] public Angle HeadYaw;
    [PacketField] public VarInt Data;
    [PacketField] public short VelocityX;
    [PacketField] public short VelocityY;
    [PacketField] public short VelocityZ;
}