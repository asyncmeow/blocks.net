using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x02, true, "Play")]
public partial class EntityAnimation : IPacket
{
    public enum Animation
    {
        SwingMainArm = 0,
        LeaveBed = 2,
        SwingOffhand = 3,
        CriticalEffect = 4,
        MagicCriticalEffect = 5
    }
    [PacketField] public VarInt EntityId;
    [PacketEnum(typeof(byte))] public Animation AnimationId;
}