using Blocks.Net.DataTypes;
using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.EntityMetadata;
using Blocks.Net.Packets.SubPackets.Slots;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Enums;

[FieldedEnum(typeof(VarInt))]
public enum EntityDataValue
{
    Byte,
    VarInt,
    VarLong,
    Float,
    String,
    TextComponent,
    OptionalTextComponent,
    Slot,
    Boolean,
    Rotations,
    Position,
    OptionalPosition,
    Direction,
    OptionalLivingEntityReference,
    BlockState,
    OptionalBlockState,
    Nbt,
    Particle,
    Particles,
    VillagerData,
    OptionalVarInt,
    Pose,
    CatVariant,
    CowVariant,
    WolfVariant,
    WolfSoundVariant,
    FrogVariant,
    PigVariant,
    ChickenVariant,
    OptionalGlobalPosition,
    PaintingVariant,
    SnifferState,
    ArmadilloState,
    Vector3,
    Quaternion
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueByte
{
    [PacketField] public sbyte Value;
}


[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueVarInt
{
    [PacketField] public VarInt Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueVarLong
{
    [PacketField] public VarLong Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueFloat
{
    [PacketField] public float Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueString
{
    [PacketField] public string Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueTextComponent
{
    [PacketField] public TextComponent Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueOptionalTextComponent
{
    [PacketField] public TextComponent? Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueSlot
{
    [PacketField] public Slot Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueBoolean
{
    [PacketField] public bool Value;
}


[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueRotations
{
    [PacketField] public float X;
    [PacketField] public float Y;
    [PacketField] public float Z;
}


[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValuePosition
{
    [PacketField] public Position Value;
}


[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueOptionalPosition
{
    [PacketField] public Position? Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueDirection
{
    [PacketEnum(typeof(VarInt))] public Direction Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueOptionalLivingEntityReference
{
    [PacketField] public Guid? Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueBlockState
{
    [PacketField] public BlockState Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueOptionalBlockState
{
    [PacketField] public BlockState Value = new(0);
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueNbt
{
    [PacketField] public NbtTag Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueParticle
{
    [PacketField] public ParticleImpl Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueParticles
{
    [PacketField] public ParticleImpl[] Value;
}


[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueOptionalVarInt
{
    [PacketField] private VarInt _value;

    public int? Value
    {
        get => _value == 0 ? null : _value - 1;
        set => _value = value is {} val ? val + 1 : 0;
    }
}



[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValuePose
{
    public enum Poses
    {
        Standing,
        FallFlying,
        Sleeping,
        Swimming,
        SpinAttack,
        Sneaking,
        LongJumping,
        Dying,
        Croaking,
        UsingTongue,
        Sitting,
        Roaring,
        Sniffing,
        Emerging,
        Digging,
        Sliding,
        Shooting,
        Inhaling
    }
    
    [PacketEnum(typeof(VarInt))] public Poses Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueCatVariant
{
    [PacketField] public RegistryReference Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueCowVariant
{
    [PacketField] public RegistryReference Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueWolfVariant
{
    [PacketField] public RegistryReference Value;
}


[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueWolfSoundVariant
{
    [PacketField] public RegistryReference Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueFrogVariant
{
    [PacketField] public RegistryReference Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValuePigVariant
{
    [PacketField] public RegistryReference Value;
}


[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueChickenVariant
{
    [PacketField] public RegistryReference Value;
}


[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueOptionalGlobalPosition
{
    [PacketField] public GlobalPosition? Value;
}


[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValuePaintingVariant
{
    [PacketField] public IdOrPaintingVariant Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueSnifferState
{
    public enum States
    {
        Idling,
        FeelingHappy,
        Scenting,
        Sniffing,
        Searching,
        Digging,
        Rising
    }
    
    [PacketEnum(typeof(VarInt))] public States Value;
}



[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueArmadilloState
{
    public enum States
    {
        Idle,
        Rolling,
        Scared,
        Unrolling
    }
    
    [PacketEnum(typeof(VarInt))] public States Value;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueVector3
{
    [PacketField] public float X;
    [PacketField] public float Y;
    [PacketField] public float Z;
}

[EnumField(typeof(EntityDataValue))]
public partial class EntityDataValueQuaternion
{
    [PacketField] public float X;
    [PacketField] public float Y;
    [PacketField] public float Z;
    [PacketField] public float W;
}