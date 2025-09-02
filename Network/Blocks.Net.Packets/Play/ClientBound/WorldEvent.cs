using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("level_event",true,"Play")]
public partial class WorldEvent : IPacket
{
    public enum Events
    {
        // Sounds
        DispenserDispenses = 1000,
        DispenserFailsToDispense = 1001,
        DispenserShoots = 1002,
        FireworkShot = 1004,
        FireExtinguished = 1009,
        PlayRecord = 1010,
        StopRecord = 1011,
        GhastWarns = 1015,
        GhastShoots = 1016,
        EnderDragonShoots = 1017,
        BlazeShoots = 1018,
        ZombieAttacksWoodenDoor = 1019,
        ZombieAttacksIronDoor = 1020,
        ZombieBreaksWoodenDoor = 1021,
        WitherBreaksBlock = 1022,
        WitherSpawned = 1023,
        WitherShoots = 1024,
        BatTakesOff = 1025,
        ZombieInfects = 1026,
        ZombieVillagerConverted = 1027,
        EnderDragonDies = 1028,
        AnvilDestroyed = 1029,
        AnvilUsed = 103,
        AnvilLands = 1031,
        PortalTravel = 1032,
        ChorusFlowerGrows = 1033,
        ChorusFLowerDies = 1034,
        BrewingStandBrews = 1035,
        EndPortalCreated = 1038,
        PhantomBites = 1039,
        ZombieConvertsToDrowned = 1040,
        HuskConvertsToZombieByDrowning = 1041,
        GrindstoneUsed = 1042,
        BookPageTurned = 1043,
        SmithingTableUsed = 1044,
        PointedDripstoneLanding = 1045,
        LavaDrippingOnCauldronFromDripstone = 1046,
        WaterDrippingOnCauldronFromDripstone = 1047,
        SkeletonConvertsToStray = 1048,
        CrafterSuccessfullyCraftsItem = 1049,
        CrafterFailsToCraftItem = 1050,
        
        // Particles
        ComposterComposts = 1500,
        LavaConvertsBlock = 1501,
        RedstoneTorchBurnsOut = 1502,
        EnderEyePlacedInEndPortalFrame = 1503,
        FluidDripsFromDripstone = 1504,
        BoneMealParticlesAndSound = 1505,
        
        DispenserActivationSmoke = 2000,
        BlockBreak = 2001,
        SplashPotion = 2002,
        EyeOfEnderBreak = 2003,
        SpawnerSpawnsMob = 2004,
        DragonBreath = 2006,
        InstantSplashPotion = 2007,
        EnderDragonDestroysBlock = 2008,
        WetSpongeVaporizes = 2009,
        CrafterActivationSmoke = 2010,
        BeeFertilizesPlant = 2011,
        TurtleEggPlaced = 2012,
        SmashAttack = 2013,
        
        EndGatewaySpawns = 3000,
        EnderDragonResurrected = 3001,
        ElectricSpark = 3002,
        CopperApplyWax = 3003,
        CopperRemoveWax = 3004,
        CopperScrapeOxidation = 3005,
        SculkCharge = 3006,
        SculkShriekerShriek = 3007,
        BlockFinishedBrushing = 3008,
        SnifferEggCracks = 3009,
        TrialSpawnerSpawnsMobAtSpawner = 3011,
        TrialSpawnerSpawnsMobAtSpawnLocation = 3012,
        TrialSpawnerDetectsPlayer = 3013,
        TrialSpawnerEjectsItem = 3014,
        VaultActivates = 3015,
        VaultDeactivates = 3016,
        CobwebWeaved = 3018,
        OminousTrialSpawnerDetectsPlayer = 3019,
        TrialSpawnerTurnsOminous = 3020,
        OminousItemSpawnerSpawnsItem = 3021
    }
    [PacketEnum(typeof(int))] public Events Event;
    [PacketField] public Position Location;
    [PacketField] public int Data;
    [PacketField] public bool DisableRelativeVolume;
}