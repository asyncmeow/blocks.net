using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("interact", false, "Play")]
public partial class Interact : IPacket
{
    public enum Types
    {
        Interact,
        Attack,
        InteractAt
    }

    [PacketField] public VarInt EntityId;
    [PacketEnum(typeof(VarInt))] public Types Type;

    [PacketOptionalField("Type == Types.InteractAt")]
    public Float3 Target;

    [PacketOptionalField("Type != Types.InteractAt")]
    public VarInt Hand;

    [PacketField] public bool SneakKeyPressed;
}