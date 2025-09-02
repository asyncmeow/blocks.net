using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("use_item_on", false, "Play")]
public partial class UseItemOn : IPacket
{
    public enum Hands
    {
        Main,
        Off
    }

    public enum Faces
    {
        NegativeY,
        PositiveY,
        NegativeZ,
        PositiveZ,
        NegativeX,
        PositiveX,
    }
    
    [PacketEnum(typeof(VarInt))] public Hands Hand;
    [PacketField] public Position Location;
    [PacketEnum(typeof(VarInt))] public Faces Face;
    [PacketField] public Float3 CursorPosition;
    [PacketField] public bool InsideBlock;
    [PacketField] public bool WorldBorderHit;
    [PacketField] public VarInt Sequence;
}