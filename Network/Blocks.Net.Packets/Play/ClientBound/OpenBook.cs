using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x30,true,"Play")]
public partial class OpenBook : IPacket
{
    public enum BookHand
    {
        Main = 0,
        Off = 1,
    }

    [PacketEnum(typeof(VarInt))] public BookHand Hand;

}