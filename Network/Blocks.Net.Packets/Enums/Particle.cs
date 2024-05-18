using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Enums;

[FieldedEnum(typeof(VarInt))]
public enum Particle
{
    AmbientEntityEffect = 0,
    AngryVillager = 1,
    Block = 2,
    BlockMarker = 3,
    Bubble = 4,
    Cloud = 5,
    Critical = 6,
    DamageIndicator = 7,
    DragonBreath = 8,
    DrippingLava = 9,
    FallingLava =  10,
    LandingLava = 11,
    DrippingWater = 12,
    FallingWater = 13,
    Dust = 14,
    DustColorTransition = 15,
    Effect = 16,
    ElderGuardian = 17,
    EnchantedHit = 18,
    Enchant = 19,
    EndRod = 20,
    EntityEffect = 21,
    ExplosionEmitter = 22,
    Explosion = 23,
    Gust = 24,
    GustEmitter = 25,
    SonicBoom = 26,
    FallingDust = 27,
    Firework = 28,
    Fishing = 29,
    Flame = 30,
    CherryLeaves = 31,
    SculkSoul = 32,
    SculkCharge = 33,
    SculkChargePop = 34,
    Soul = 36,
    Flash = 37,
    HappyVillager = 38,
    Composter = 39,
    Heart = 40,
    InstantEffect = 41,
    Item = 42,
    Vibration = 43,
    ItemSlime = 44,
    ItemSnowball = 45,
    LargeSmoke = 46,
    Lava = 47,
    Mycelium = 48,
    Poof = 50,
    Portal = 51,
    Rain = 52,
    Smoke = 53,
    WhiteSmoke = 54,
    Sneeze = 55,
    Spit = 56,
    SquidInk = 57,
    SweepAttack = 58,
    TotemOfUndying = 59,
    Underwater = 60,
    Splash = 61,
    Witch = 62,
    BubblePop = 63,
    CurrentDown = 64,
    BubbleColumnUp = 65,
    Nautilus = 66,
    Dolphin = 67,
    CampfireCosySmoke = 68,
    CampfireSignalSmoke = 69,
    DrippingHoney = 70,
    FallingHoney = 71,
    LandingHoney = 72,
    FallingNectar = 73,
    FallingSporeBlossom = 74,
    Ash = 75,
    CrimsonSpore = 76,
    WarpedSpore = 77,
    SporeBlossomAir = 78,
    DrippingObsidianTear = 79,
    FallingObsidianTear = 80,
    LandingObsidianTear = 81,
    ReversePortal = 82,
    WhiteAsh = 83,
    SmallFlame = 84,
    Snowflake = 85,
    DrippingDripstoneLava = 86,
    FallingDripstoneLava = 87,
    DrippingDripstoneWater = 88,
    FallingDripstoneWater = 89,
    GlowSquidInk = 90,
    Glow = 91,
    WaxOn = 92,
    WaxOff = 93,
    ElectricSpark = 94,
    Scrape = 95,
    Shriek = 96,
    EggCrack = 97,
    DustPlume = 98,
    GustDust = 99,
    TrialSpawnerDetection = 100
}

[EnumField(typeof(Particle))]
public partial class ParticleBlock
{
    [PacketField] public VarInt BlockState;
}

[EnumField(typeof(Particle))]
public partial class ParticleBlockMarker
{
    [PacketField] public VarInt BlockState;
}

[EnumField(typeof(Particle))]
public partial class ParticleDust
{
    [PacketField] public float Red;
    [PacketField] public float Green;
    [PacketField] public float Blue;
    [PacketField] public float Scale;
}

[EnumField(typeof(Particle))]
public partial class ParticleDustColorTransition
{
    [PacketField] public float FromRed;
    [PacketField] public float FromGreen;
    [PacketField] public float FromBlue;
    [PacketField] public float Scale;
    [PacketField] public float ToRed;
    [PacketField] public float ToGreen;
    [PacketField] public float ToBlue;
}

[EnumField(typeof(Particle))]
public partial class ParticleFallingDust
{
    [PacketField] public VarInt BlockState;
}

[EnumField(typeof(Particle))]
public partial class ParticleSculkCharge
{
    [PacketField] public float Roll;
}

[EnumField(typeof(Particle))]
public partial class ParticleItem
{
    [PacketField] public Slot Item;
}

[EnumField(typeof(Particle))]
public partial class ParticleShriek
{
    [PacketField] public VarInt Delay;
}