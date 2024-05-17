using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Enums;

[FieldedEnum(typeof(VarInt))]
public enum BossBarAction
{
    Add = 0x00,
    Remove = 0x01,
    UpdateHealth = 0x02,
    UpdateTitle = 0x03,
    UpdateStyle = 0x04,
    UpdateFlags = 0x05,
}

public enum BossBarColor
{
    Pink = 0x00,
    Blue = 0x01,
    Red = 0x02,
    Green = 0x03,
    Yellow = 0x04,
    Purple = 0x05,
    White = 0x06
}

public enum BossBarDividers
{
    None = 0x00,
    Six = 0x01,
    Ten = 0x02,
    Twelve = 0x03,
    Twenty = 0x04
}

[Flags]
public enum BossBarFlags
{
    Darken = 0x01,
    Music = 0x02,
    Fog = 0x04
}

[EnumField(typeof(BossBarAction))]
public partial class BossBarActionAdd
{
    [PacketField] public NbtTag Title;
    [PacketField] public float Health;
    [PacketEnum(typeof(VarInt))] public BossBarColor Color;
    [PacketEnum(typeof(VarInt))] public BossBarDividers Dividers;
    [PacketEnum(typeof(byte))] public BossBarFlags Flags;
}

[EnumField(typeof(BossBarAction))]
public partial class BossBarActionUpdateHealth
{
    [PacketField] public float Health;
}

[EnumField(typeof(BossBarAction))]
public partial class BossBarActionUpdateTitle
{
    [PacketField] public NbtTag Title;
}

[EnumField(typeof(BossBarAction))]
public partial class BossBarActionUpdateStyle
{
    [PacketEnum(typeof(byte))] public BossBarColor Color;
    [PacketEnum(typeof(byte))] public BossBarDividers Dividers;
}

[EnumField(typeof(BossBarAction))]
public partial class BossBarActionUpdateFlags
{
    [PacketEnum(typeof(byte))] public BossBarFlags Flags;
}