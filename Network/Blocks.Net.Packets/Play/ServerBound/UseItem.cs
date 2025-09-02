using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("use_item",false,"Play")]
public partial class UseItem : IPacket
{
    public enum Hands
    {
        Main,
        Off
    }
    [PacketEnum(typeof(VarInt))] public Hands Hand;
    [PacketField] public VarInt Sequence;
    [PacketField] public float Yaw;
    [PacketField] public float Pitch;
}