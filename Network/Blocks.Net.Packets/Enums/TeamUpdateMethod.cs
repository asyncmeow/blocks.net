using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Enums;

[FieldedEnum(typeof(byte))]
public enum TeamUpdateMethod : byte
{
    CreateTeam,
    RemoveTeam,
    UpdateTeamInfo,
    AddEntitiesToTeam,
    RemoveEntitiesFromTeam,
}

[Flags]
public enum TeamFriendlyFlags : byte
{
    AllowFriendlyFire = 0x01,
    CanSeeInvis = 0x02
}

public enum TeamNameTagVisibility
{
    Always,
    Never,
    HideForOtherTeams,
    HideForOwnTeam
}


public enum TeamCollisionRules
{
    Always,
    Never,
    PushOtherTeams,
    PushOwnTeam
}

public enum TeamColors
{
    Black,
    DarkBlue,
    DarkGreen,
    DarkAqua,
    DarkRed,
    DarkPurple,
    Gold,
    Gray,
    DarkGray,
    Blue,
    Green,
    Aqua,
    LightPurple,
    Yellow,
    White,
    Obfuscated,
    Bold,
    Strikethrough,
    Underlined,
    Italic,
    Reset
}

[EnumField(typeof(TeamUpdateMethod))]
public partial class CreateTeam
{
    [PacketField] public TextComponent TeamDisplayName;
    [PacketEnum(typeof(byte))] public TeamFriendlyFlags FriendlyFlags;
    [PacketEnum(typeof(VarInt))] public TeamNameTagVisibility NameTagVisibility;
    [PacketEnum(typeof(VarInt))] public TeamCollisionRules CollisionRule;
    [PacketEnum(typeof(VarInt))] public TeamColors TeamColor;
    [PacketField] public string[] Entities;
}

[EnumField(typeof(TeamUpdateMethod))]
public partial class UpdateTeamInfo
{
    [PacketField] public TextComponent TeamDisplayName;
    [PacketEnum(typeof(byte))] public TeamFriendlyFlags FriendlyFlags;
    [PacketEnum(typeof(VarInt))] public TeamNameTagVisibility NameTagVisibility;
    [PacketEnum(typeof(VarInt))] public TeamCollisionRules CollisionRule;
    [PacketEnum(typeof(VarInt))] public TeamColors TeamColor;
    [PacketField] public TextComponent TeamPrefix;
    [PacketField] public TextComponent TeamSuffix;
}

[EnumField(typeof(TeamUpdateMethod))]
public partial class AddEntitiesToTeam
{
    [PacketField] public string[] Entities;
}


[EnumField(typeof(TeamUpdateMethod))]
public partial class RemoveEntitiesFromTeam
{
    [PacketField] public string[] Entities;
}