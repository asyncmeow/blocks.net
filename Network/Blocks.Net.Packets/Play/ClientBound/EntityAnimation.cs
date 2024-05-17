using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x03,true,"Play")]
public partial class EntityAnimation : IPacket
{
    public enum AnimationType
    {
        SwingMainArm = 0x00,
        LeaveBed = 0x02,
        SwingOffhand = 0x03,
        Critical = 0x04,
        MagicCritical = 0x05
    }

    [PacketField] public VarInt Id;
    [PacketEnum(typeof(UnsignedByte))] public AnimationType Animation;
}