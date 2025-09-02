using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("player_abilities",true,"Play")]
public partial class PlayerAbilities : IPacket
{
    [Flags]
    public enum AbilitiesFlags : byte
    {
        Invulnerable = 1,
        Flying = 2,
        AllowFlying = 4,
        InstantBreak = 8
    }
    
    [PacketEnum(typeof(byte))] public AbilitiesFlags Flags;
    [PacketField] public float FlyingSpeed = 0.05f;
    [PacketField] public float FovModifier = 0.1f;
}