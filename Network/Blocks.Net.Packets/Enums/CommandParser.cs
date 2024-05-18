using System.Reflection;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Enums;

[FieldedEnum(typeof(VarInt))]
public enum CommandParser
{
    Bool = 0,
    Float = 1,
    Double = 2,
    Integer = 3,
    Long = 4,
    String =  5,
    Entity = 6,
    GameProfile = 7,
    BlockPos = 8,
    ColumnPos = 9,
    Vec3 = 10,
    Vec2 = 11,
    BlockState = 12,
    BlockPredicate = 13,
    ItemStack = 14,
    ItemPredicate = 15,
    Color = 16,
    Component = 17,
    Style = 18,
    Message = 19,
    Nbt = 20,
    NbtTag = 21,
    NbtPath = 22,
    Objective = 23,
    ObjectiveCriteria = 24,
    Operation = 25,
    Particle = 26,
    Angle = 27,
    Rotation = 28,
    ScoreboardSlot = 29,
    ScoreHolder = 30,
    Swizzle = 31,
    Team = 32,
    ItemSlot = 33,
    ResourceLocation = 34,
    Function = 35,
    EntityAnchor = 36,
    IntRang = 37,
    FloatRange = 38,
    Dimension = 39,
    GameMode = 40,
    Time = 41,
    ResourceOrTag = 42,
    ResourceOrTagKey = 43,
    Resource = 44,
    ResourceKey = 45,
    TemplateMirror = 46,
    TemplateDirection = 47,
    HeightMap = 48,
    Uuid = 49
}

[EnumField(typeof(CommandParser))]
public partial class CommandParserDouble
{
    [PacketField] public byte Flags;

    [PacketOptionalField("(Flags & 0x01) != 0")]
    public double Min;

    [PacketOptionalField("(Flags & 0x02) != 0")]
    public double Max;
}

[EnumField(typeof(CommandParser))]
public partial class CommandParserFloat
{
    [PacketField] public byte Flags;

    [PacketOptionalField("(Flags & 0x01) != 0")]
    public float Min;

    [PacketOptionalField("(Flags & 0x02) != 0")]
    public float Max;
}


[EnumField(typeof(CommandParser))]
public partial class CommandParserInteger
{
    [PacketField] public byte Flags;

    [PacketOptionalField("(Flags & 0x01) != 0")]
    public int Min;

    [PacketOptionalField("(Flags & 0x02) != 0")]
    public int Max;
}

[EnumField(typeof(CommandParser))]
public partial class CommandParserLong
{
    [PacketField] public byte Flags;

    [PacketOptionalField("(Flags & 0x01) != 0")]
    public long Min;

    [PacketOptionalField("(Flags & 0x02) != 0")]
    public long Max;
}


[EnumField(typeof(CommandParser))]
public partial class CommandParserString
{
    public enum StringType : byte
    {
        SingleWord = 0,
        QuotablePhrase = 1,
        GreedyPhrase = 2
    }

    [PacketEnum(typeof(VarInt))] public StringType Type;
}

[EnumField(typeof(CommandParser))]
public partial class CommandParserEntity
{
    [PacketField] public byte Flags;
}

[EnumField(typeof(CommandParser))]
public partial class CommandParserScoreHolder
{
    [PacketField] public bool AllowMultiple;
}

[EnumField(typeof(CommandParser))]
public partial class CommandParserTime
{
    [PacketField] public int Min;
}

[EnumField(typeof(CommandParser))]
public partial class CommandParserResourceOrTag
{
    [PacketField] public Identifier Registry;
}

[EnumField(typeof(CommandParser))]
public partial class CommandParserResourceOrTagKey
{
    [PacketField] public Identifier Registry;
}

[EnumField(typeof(CommandParser))]
public partial class CommandParserResource
{
    [PacketField] public Identifier Registry;
}

[EnumField(typeof(CommandParser))]
public partial class CommandParserResourceKey
{
    [PacketField] public Identifier Registry;
}