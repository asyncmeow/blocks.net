using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_health",true,"Play")]
public partial class SetHealth : IPacket
{
    [PacketField] public float Health;
    [PacketField] public VarInt Food;
    [PacketField] public float FoodSaturation;
}