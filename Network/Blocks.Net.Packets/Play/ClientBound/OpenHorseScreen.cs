using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;


[Packet(0x21,true,"Play")]
public partial class OpenHorseScreen : IPacket
{
    [PacketField] public byte WindowId;
    [PacketField] public VarInt SlotCount;
    [PacketField] public int EntityId;
}