using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("move_entity_pos_rot",true,"Play")]
public partial class UpdateEntityPositionAndRotation : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public FixedShort3 Delta;
    [PacketField] public Angle Yaw;
    [PacketField] public Angle Pitch;
    [PacketField] public bool OnGround;
}