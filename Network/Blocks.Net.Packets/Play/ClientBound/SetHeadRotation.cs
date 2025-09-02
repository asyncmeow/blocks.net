using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("rotate_head",true,"Play")]
public partial class SetHeadRotation : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public Angle HeadYaw;
}