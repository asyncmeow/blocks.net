using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x2C,true,"Play")]
public partial class UpdateEntityPosition : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public short DeltaX;
    [PacketField] public short DeltaY;
    [PacketField] public short DeltaZ;
    [PacketField] public bool OnGround;
}