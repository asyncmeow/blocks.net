using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_entity_motion",true,"Play")]
public partial class SetEntityVelocity : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public Short3 Velocity;
}