using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.Packets.SubPackets.Slots;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Enums;

[FieldedEnum(typeof(VarInt))]
public enum Particle
{
    AngryVillager,
    Block,
    BlockMarker,
    Bubble,
    Cloud,
    Crit,
    DamageIndicator,
    DragonBreath,
    DrippingLava,
    FallingLava,
    LandingLava,
    DrippingWater,
    FallingWater,
    Dust,
    DustColorTransition,
    Effect,
    ElderGuardian,
    EnchantedHit,
    Enchant,
    EndRod,
    EntityEffect,
    ExplosionEmitter,
    Explosion,
    Gust,
    SmallGust,
    GustEmitterLarge,
    GustEmitterSmall,
    SonicBoom,
    FallingDust,
    Firework,
    Fishing,
    Flame,
    Infested,
    CherryLeaves,
    PaleOakLeaves,
    TintedLeaves,
    SculkSoul,
    SculkCharge,
    SculkChargePop,
    SoulFireFlame,
    Soul,
    Flash,
    HappyVillager,
    Composter,
    Heart,
    InstantEffect,
    Item,
    Vibration,
    Trail,
    ItemSlime,
    ItemCobweb,
    ItemSnowball,
    LargeSmoke,
    Lava,
    Mycelium,
    Note,
    Poof,
    Portal,
    Rain,
    Smoke,
    WhiteSmoke,
    Sneeze,
    Spit,
    SquidInk,
    SweepAttack,
    TotemOfUndying,
    Underwater,
    Splash,
    Witch,
    BubblePop,
    CurrentDown,
    BubbleColumnUp,
    Nautilus,
    Dolphin,
    CampfireCosySmoke,
    CampfireSignalSmoke,
    DrippingHoney,
    FallingHoney,
    LandingHoney,
    FallingNectar,
    FallingSporeBlossom,
    Ash,
    CrimsonSpore,
    WarpedSpore,
    SporeBlossomAir,
    DrippingObsidianTear,
    FallingObsidianTear,
    LandingObsidianTear,
    ReversePortal,
    WhiteAsh,
    SmallFlame,
    SnowFlake,
    DrippingDripstoneLava,
    FallingDripstoneLava,
    DrippingDripstoneWater,
    FallingDripstoneWater,
    GlowSquidInk,
    Glow,
    WaxOn,
    WaxOff,
    ElectricSpark,
    Scrape,
    Shriek,
    EggCrack,
    DustPlume,
    TrialSpawnerDetection,
    TrialSpawnerDetectionOminous,
    VaultConnection,
    DustPillar,
    OminousSpawning,
    RaidOmen,
    TrialOmen,
    BlockCrumble,
    Firefly
}

[EnumField(typeof(Particle))]
public partial class ParticleBlock
{
    [PacketField] public BlockState Block;
}

[EnumField(typeof(Particle))]
public partial class ParticleBlockMarker
{
    [PacketField] public BlockState Block;
}

[EnumField(typeof(Particle))]
public partial class ParticleDust
{
    [PacketField] public int Color;
    [PacketField] public float Scale;
}

[EnumField(typeof(Particle))]
public partial class ParticleDustColorTransition
{
    
    [PacketField] public int FromColor;
    [PacketField] public int ToColor;
    [PacketField] public float Scale;
}

[EnumField(typeof(Particle))]
public partial class ParticleEntityEffect
{
    [PacketField] public int Color;
}


[EnumField(typeof(Particle))]
public partial class ParticleFallingDust
{
    [PacketField] public BlockState Block;
}

[EnumField(typeof(Particle))]
public partial class ParticleTintedLeaves
{
    [PacketField] public int Color;
}

[EnumField(typeof(Particle))]
public partial class ParticleSculkCharge
{
    [PacketField] public int Roll;
}


[EnumField(typeof(Particle))]
public partial class ParticleItem
{
    [PacketField] public Slot Item;
}

[EnumField(typeof(Particle))]
public partial class ParticleVibration
{
    public const int SourceTypeBlock = 0;
    public const int SourceTypeEntity = 1;
    [PacketField] public VarInt SourceType;

    [PacketOptionalField("SourceType == SourceTypeBlock")]
    public Position BlockPosition;

    [PacketOptionalField("SourceType == SourceTypeEntity")]
    public VarInt EntityId;

    [PacketOptionalField("SourceType == SourceTypeEntity")]
    public float EntityEyeHeight;

    [PacketField] public VarInt Ticks;
}

[EnumField(typeof(Particle))]
public partial class ParticleTrail
{
    [PacketField] public Double3 Target;
    [PacketField] public int Color;
    [PacketField] public VarInt Duration;
}

[EnumField(typeof(Particle))]
public partial class ParticleShriek
{
    [PacketField] public VarInt Delay;
}

[EnumField(typeof(Particle))]
public partial class ParticleDustPillar
{
    [PacketField] public BlockState Block;
}


[EnumField(typeof(Particle))]
public partial class ParticleBlockCrumble
{
    [PacketField] public BlockState Block;
}