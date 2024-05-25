using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x2E,true,"Play")]
public partial class UpdateEntityRotation : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public Angle Yaw;
    [PacketField] public Angle Pitch;
    [PacketField] public bool OnGround;
}