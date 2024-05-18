using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x22,true,"Play")]
public partial class HurtAnimation : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public float Yaw;
}