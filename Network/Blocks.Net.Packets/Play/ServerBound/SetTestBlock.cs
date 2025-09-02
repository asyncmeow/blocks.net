using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("set_test_block",false,"Play")]
public partial class SetTestBlock : IPacket
{
    public enum Modes
    {
        Start,
        Log,
        Fail,
        Accept
    }
    [PacketField] public Position Location;
    [PacketEnum(typeof(VarInt))] public Modes Mode;
    [PacketField] public string Message;
}