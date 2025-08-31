using System.Runtime.CompilerServices;
using Blocks.Net.DataTypes;
using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.Packets.SubPackets.EntityMetadata;
using Blocks.Net.Packets.SubPackets.Slots;
using Blocks.Net.Packets.SubPackets.Slots.Components;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Enums;

[FieldedEnum(typeof(VarInt))]
public enum StructuredComponent
{
    CustomData,
    MaxStackSize,
    MaxDamage,
    Damage,
    Unbreakable,
    CustomName,
    ItemName,
    ItemModel,
    Lore,
    Rarity,
    Enchantments,
    CanPlaceOn,
    CanBreak,
    AttributeModifiers,
    CustomModelData,
    TooltipDisplay,
    RepairCost,
    CreativeSlotLock,
    EnchantmentGlintOverride,
    IntangibleProjectile,
    Food,
    Consumable,
    UseRemainder,
    UseCooldown,
    DamageResistant,
    Tool,
    Weapon,
    Enchantable,
    Equippable,
    Repairable,
    Glider,
    TooltipStyle,
    DeathProtection,
    BlocksAttacks,
    StoredEnchantments,
    DyedColor,
    MapColor,
    MapId,
    MapDecorations,
    MapPostProcessing,
    ChargedProjectiles,
    BundleContents,
    PotionContents,
    PotionDurationScale,
    SuspiciousStewEffects,
    WritableBookContent,
    WrittenBookContent,
    Trim,
    DebugStickState,
    EntityData,
    BucketEntityData,
    BlockEntityData,
    Instrument,
    ProvidesTrimMaterial,
    OminousBottleAmplifier,
    JukeboxPlayable,
    ProvidesBannerPatterns,
    Recipes,
    LodestoneTracker,
    FireworkExplosion,
    Fireworks,
    Profile,
    NoteBlockSound,
    BannerPatterns,
    BaseColor,
    PotDecorations,
    Container,
    BlockState,
    Bees,
    Lock,
    ContainerLoot,
    BreakSound,
    VillagerVariant,
    WolfVariant,
    WolfSoundVariant,
    WolfCollar,
    FoxVariant,
    SalmonSize,
    ParrotVariant,
    TropicalFishPattern,
    TropicalFishBaseColor,
    TropicalFishPatternColor,
    MooshroomVariant,
    RabbitVariant,
    PigVariant,
    CowVariant,
    ChickenVariant,
    FrogVariant,
    HorseVariant,
    PaintingVariant,
    LlamaVariant,
    AxolotlVariant,
    CatVariant,
    CatCollar,
    SheepColor,
    ShulkerColor
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentCustomData
{
    [PacketField] public NbtTag Data;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentMaxStackSize
{
    [PacketField] public VarInt MaxStackSize;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentMaxDamage
{
    [PacketField] public VarInt MaxDamage;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentDamage
{
    [PacketField] public VarInt Damage;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentCustomName
{
    [PacketField] public NbtTag Name;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentItemName
{
    [PacketField] public NbtTag Name;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentItemModel
{
    [PacketField] public Identifier Model;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentLore
{
    [PacketField] public NbtTag[] Lines;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentRarity
{
    public enum RarityEnum
    {
        Common,
        Uncommon,
        Rare,
        Epic
    }

    [PacketEnum(typeof(VarInt))] public RarityEnum Rarity;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentEnchantments
{
    [PacketField] public Enchantment[] Enchantments;
}

[EnumField(typeof(StructuredComponent))]
public partial class CanPlaceOn
{
    [PacketField] public BlockPredicate[] BlockPredicates;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentCanBreak
{
    [PacketField] public BlockPredicate[] BlockPredicates;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentAttributeModifiers
{
    [PacketField] public AttributeModifier[] Modifiers;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentCustomModelData
{
    [PacketField] public float[] Floats;
    [PacketField] public bool[] Flags;
    [PacketField] public string[] Strings;
    [PacketField] public int[] Colors;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentTooltipDisplay
{
    [PacketField] public bool HideTooltip;
    [PacketField] public RegistryReference[] HiddenComponents;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentRepairCost
{
    [PacketField] public VarInt Cost;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentEnchantmentGlintOverride
{
    [PacketField] public bool HasGlint;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentIntangibleProjectile
{
    [PacketField] public NbtTag Empty;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentFood
{
    [PacketField] public VarInt Nutrition;
    [PacketField] public float SaturationModifier;
    [PacketField] public bool CanAlwaysEat;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentConsumable
{
    public enum AnimationEnum
    {
        None,
        Eat,
        Drink,
        Block,
        Bow,
        Spear,
        Crossbow,
        Spyglass,
        TootHorn,
        Brush
    }

    [PacketField] public float ConsumeSeconds;
    [PacketField] public IdOrSoundEvent Sound;
    [PacketField] public bool HasConsumeParticles;
    [PacketField] public ConsumeEffect[] Effects;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentUseRemainder
{
    [PacketField] public Slot Remainder;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentUseCooldown
{
    [PacketField] public float Seconds;
    [PacketField] public Identifier? CooldownGroup;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentDamageResistant
{
    [PacketField] public Identifier Types;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentTool
{
    [PacketField] public ToolRule[] Rules;
    [PacketField] public float DefaultMiningSpeed;
    [PacketField] public VarInt DamagePerBlock;
    [PacketField] public bool CanDestroyBlocksInCreative;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentWeapon
{
    [PacketField] public VarInt DamagePerAttack;
    [PacketField] public float DisableBlockingForSeconds;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentEnchantable
{
    [PacketField] public VarInt Value;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentEquippable
{
    public enum SlotEnum
    {
        MainHand,
        Feet,
        Legs,
        Chest,
        Head,
        Offhand,
        Body
    }

    [PacketEnum(typeof(VarInt))] public SlotEnum Slot;
    [PacketField] public IdOrSoundEvent EquipSound;
    [PacketField] public Identifier? Model;
    [PacketField] public Identifier? CameraOverlay;
    [PacketField] public IdSet? AllowedEntities;
    [PacketField] public bool Dispensable;
    [PacketField] public bool Swappable;
    [PacketField] public bool DamageOnHurt;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentRepairable
{
    [PacketField] public IdSet Items;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentTooltipStyle
{
    [PacketField] public Identifier Style;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentDeathProtection
{
    [PacketField] public ConsumeEffect[] Effects;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentBlocksAttacks
{
    [PacketField] public float BlockDelaySeconds;
    [PacketField] public float DisableCooldownScale;
    [PacketField] public DamageReduction[] DamageReduction;
    [PacketField] public float ItemDamageThreshold;
    [PacketField] public float ItemDamageFactor;
    [PacketField] public Identifier? BypassedBy;
    [PacketField] public IdOrSoundEvent BlockSound;
    [PacketField] public IdOrSoundEvent DisableSound;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentStoredEnchantments
{
    [PacketField] public Enchantment[] Enchantments;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentDyedColor
{
    [PacketField] public int Color;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentMapColor
{
    [PacketField] public int Color;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentMapId
{
    [PacketField] public VarInt Id;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentMapDecorations
{
    [PacketField] public NbtTag Data;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentMapPostProcessing
{
    public enum PostProcessingType
    {
        Lock,
        Scale
    }

    [PacketEnum(typeof(VarInt))] public PostProcessingType Type;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentChargedProjectiles
{
    [PacketField] public Slot[] Projectiles;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentBundleContents
{
    [PacketField] public Slot[] Items;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentPotionContents
{
    [PacketField] public RegistryReference? PotionId;
    [PacketField] public int? Color;
    [PacketField] public PotionEffect[] CustomEffects;
    [PacketField] public string CustomName;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentPotionDurationScale
{
    [PacketField] public float EffectMultiplier;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentSuspiciousStewEffects
{
    [PacketField] public SuspiciousStewEffect[] Effects;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentWritableBookContent
{
    [PacketField] public BookPage[] Pages;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentWrittenBookContent
{
    [PacketField] public string RawTitle;
    [PacketField] public string? FilteredTitle;
    [PacketField] public string Author;
    [PacketField] public VarInt Generation;
    [PacketField] public BookPage[] Pages;
    [PacketField] public bool Resolved;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentTrim
{
    [PacketField] public IdOrTrimMaterial TrimMaterial;
    [PacketField] public IdOrTrimPattern TrimPattern;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentDebugStickState
{
    [PacketField] public NbtTag Data;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentEntityData
{
    [PacketField] public NbtTag Data;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentBucketEntityData
{
    [PacketField] public NbtTag Data;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentBlockEntityData
{
    [PacketField] public NbtTag Data;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentInstrument
{
    [PacketField] public IdOrInstrument Instrument;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentProvidesTrimMaterial
{
    [PacketField] public bool IsDirectDefinition;

    [PacketOptionalField("!IsDirectDefinition")]
    public Identifier ReferencedMaterialName;

    [PacketOptionalField("IsDirectDefinition")]
    public IdOrTrimMaterial DirectMaterialDefinition;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentOminousBottleAmplifier
{
    [PacketField] public VarInt Amplifier;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentJukeboxPlayable
{
    [PacketField] public bool IsDirectDefinition;

    [PacketOptionalField("!IsDirectDefinition")]
    public Identifier ReferencedSongName;

    [PacketOptionalField("IsDirectDefinition")]
    public IdOrJukeboxSong DirectSongDefinition;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentProvidesBannerPatterns
{
    [PacketField] public Identifier Pattern;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentRecipes
{
    [PacketField] public NbtTag Data;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentLodestoneTracker
{
    [PacketField] public bool HasGlobalPosition;

    [PacketOptionalField(nameof(HasGlobalPosition))]
    public Identifier Dimension;

    [PacketOptionalField(nameof(HasGlobalPosition))]
    public Position Position;

    [PacketField] public bool Tracked;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentFireworkExplosion
{
    [PacketField] public FireworkExplosion Explosion;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentFireworks
{
    [PacketField] public VarInt FlightDuration;
    [PacketField] public FireworkExplosion[] Explosions;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentProfile
{
    [PacketField] public string? Name;
    [PacketField] public Uuid? Uuid;
    [PacketField] public PlayerProperty[] Properties;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentNoteBlockSound
{
    [PacketField] public Identifier Sound;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentBannerPatterns
{
    [PacketField] public BannerLayer[] Layers;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentBaseColor
{
    [PacketField] public DyeColor BaseColor;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentPotDecorations
{
    [PacketField] public RegistryReference[] Decorations;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentContainer
{
    [PacketField] public Slot[] Items;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentBlockState
{
    [PacketField] public BlockStateProperty[] Properties;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentBees
{
    [PacketField] public Bee[] Bees;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentLock
{
    [PacketField] public NbtTag Key;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentContainerLoot
{
    [PacketField] public NbtTag Data;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentBreakSound
{
    [PacketField] public IdOrSoundEvent SoundEvent;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentVillagerVariant
{
    [PacketField] public RegistryReference Variant;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentWolfVariant
{
    [PacketField] public RegistryReference Variant;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentWolfSoundVariant
{
    [PacketField] public RegistryReference Variant;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentWolfCollar
{
    [PacketField] public DyeColor Color;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentFoxVariant
{
    public enum Variants
    {
        Red,
        Snow
    }

    [PacketField] public Variants Variant;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentSalmonSize
{
    public enum Sizes
    {
        Small,
        Medium,
        Large
    }

    [PacketField] public Sizes Size;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentParrotVariant
{
    [PacketField] public RegistryReference Variant;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentTropicalFishPattern
{
    public enum Patterns
    {
        Kob,
        Sunstreak,
        Snooper,
        Dasher,
        Brinely,
        Spotty,
        Flooper,
        Stripey,
        Glitter,
        Blockfish,
        Betty,
        Clayfish
    }

    [PacketField] public Patterns Pattern;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentTropicalFishBaseColor
{
    [PacketField] public DyeColor Color;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentTropicalFishPatternColor
{
    [PacketField] public DyeColor Color;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentMooshroomVariant
{
    public enum Variants
    {
        Red,
        Brown
    }

    [PacketField] public Variants Variant;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentRabbitVariant
{
    public enum Variants
    {
        Brown,
        White,
        Black,
        WhiteSplotched,
        Gold,
        Salt,
        Evil
    }

    [PacketField] public Variants Variant;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentPigVariant
{
    [PacketField] public RegistryReference Variant;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentCowVariant
{
    [PacketField] public RegistryReference Variant;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentChickenVariant
{
    [PacketField] public bool IsRegistryReference;

    [PacketOptionalField("!IsRegistryReference")]
    public Identifier VariantName;

    [PacketOptionalField("IsRegistryReference")]
    public RegistryReference VariantReference;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentFrogVariant
{
    [PacketField] public RegistryReference Variant;
}


[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentHorseVariant
{
    public enum Variants
    {
        White,
        Creamy,
        Chestnut,
        Brown,
        Black,
        Gray,
        DarkBrown
    }

    [PacketField] public Variants Variant;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentPaintingVariant
{
    [PacketField] public PaintingVariant Variant;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentLlamaVariant
{
    public enum Variants
    {
        Creamy,
        White,
        Brown,
        Gray
    }

    [PacketField] public Variants Variant;
}


[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentAxolotlVariant
{
    public enum Variants
    {
        Lucy,
        Wild,
        Gold,
        Cyan,
        Blue
    }

    [PacketField] public Variants Variant;
}


[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentCatVariant
{
    [PacketField] public RegistryReference Variant;
}



[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentCatCollar
{
    [PacketField] public DyeColor Color;
}

[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentSheepColor
{
    [PacketField] public DyeColor Color;
}


[EnumField(typeof(StructuredComponent))]
public partial class StructuredComponentShulkerColor
{
    [PacketField] public DyeColor Color;
}
