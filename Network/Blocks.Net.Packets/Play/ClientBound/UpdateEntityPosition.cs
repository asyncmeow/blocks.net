using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("move_entity_pos", true, "Play")]
public partial class UpdateEntityPosition : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public FixedShort3 Delta;
    [PacketField] public bool OnGround;
}