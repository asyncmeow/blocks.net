using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("horse_screen_open",true,"Play")]
public partial class OpenHorseScreen : IPacket
{
    [PacketField] public VarInt WindowId;
    [PacketField] public VarInt InventoryColumnsCount;
    [PacketField] public int EntityId;
}