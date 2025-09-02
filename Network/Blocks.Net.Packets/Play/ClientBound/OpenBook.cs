using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("open_book",true,"Play")]
public partial class OpenBook : IPacket
{
    public enum Hands
    {
        MainHand,
        OffHand
    }
    [PacketEnum(typeof(VarInt))] public Hands Hand;
}