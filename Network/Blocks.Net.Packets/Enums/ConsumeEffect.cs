using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.Packets.SubPackets.Slots.Components;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Enums;

[FieldedEnum(typeof(VarInt))]
public enum ConsumeEffect
{
    ApplyEffects,
    RemoveEffects,
    ClearAllEffects,
    TeleportRandomly,
    PlaySound
}

[EnumField(typeof(ConsumeEffect))]
public partial class ConsumeEffectApplyEffects
{
    [PacketField] public PotionEffect[] Effects;
    [PacketField] public float Probability;
}

[EnumField(typeof(ConsumeEffect))]
public partial class ConsumeEffectRemoveEffects
{
    [PacketField] public IdSet Effects;
}

[EnumField(typeof(ConsumeEffect))]
public partial class ConsumeEffectTeleportRandomly
{
    [PacketField] public float Diameter;
}

[EnumField(typeof(ConsumeEffect))]
public partial class ConsumeEffectPlaySound
{
    [PacketField] public SoundEvent Sound;
}