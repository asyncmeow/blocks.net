using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x2D,true,"Play")]
public partial class UpdateEntityPositionAndRotation : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public short DeltaX;
    [PacketField] public short DeltaY;
    [PacketField] public short DeltaZ;
    [PacketField] public Angle Yaw;
    [PacketField] public Angle Pitch;
    [PacketField] public bool OnGround;
}