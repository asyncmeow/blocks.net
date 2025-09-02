using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("swing",false,"Play")]
public partial class SwingArm : IPacket
{
    public enum Hands
    {
        Main,
        Off
    }

    [PacketEnum(typeof(VarInt))] public Hands Hand;
}