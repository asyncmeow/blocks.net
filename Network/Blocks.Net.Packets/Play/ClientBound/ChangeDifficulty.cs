using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x0A, true, "Play")]
public partial class ChangeDifficulty : IPacket
{
    public enum DifficultyEnum
    {
        Peaceful = 0,
        Easy = 1,
        Normal = 2,
        Hard = 3
    }

    [PacketEnum(typeof(VarInt))] public DifficultyEnum Difficulty;

    [PacketField] public bool DifficultyLocked;
}