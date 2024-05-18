using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x0B,true,"Play")]
public partial class ChangeDifficulty : IPacket
{
    public enum DifficultyEnum : byte
    {
        Peaceful = 0,
        Easy = 1,
        Normal = 2,
        Hard = 3
    }

    [PacketEnum(typeof(byte))] public DifficultyEnum Difficulty;
    [PacketField] public bool Locked;
}