using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x36,true,"Play")]
public partial class PlayerAbilities : IPacket
{
    [Flags]
    public enum PlayerAbilityFlags
    {
        Invulnerable = 0x01,
        Flying = 0x02,
        AllowFlying = 0x04,
        InstantBreak = 0x10
    }

    [PacketEnum(typeof(byte))] public PlayerAbilityFlags Flags;
    [PacketField] public float FlyingSpeed;
    [PacketField] public float FieldOfViewModifier;
}