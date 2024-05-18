using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x29,true,"Play")]
public partial class Login : IPacket
{
    public enum LoginGameMode
    {
        Survival = 0,
        Creative = 1,
        Adventure = 2,
        Spectator = 3
    }
    [PacketField] public int EntityId;
    [PacketField] public bool IsHardcore;
    [PacketField] public VarInt DimensionCount;
    [PacketArrayField("DimensionCount")] public Identifier[] DimensionNames;
    [PacketField] public VarInt MaxPlayers;
    [PacketField] public VarInt ViewDistance;
    [PacketField] public VarInt SimulationDistance;
    [PacketField] public bool ReducedDebugInfo;
    [PacketField] public bool EnableRespawnScreen;
    [PacketField] public bool DoLimitedCrafting;
    [PacketField] public Identifier DimensionType;
    [PacketField] public Identifier DimensionName;
    [PacketField] public long HashedSeed;
    [PacketEnum(typeof(byte))] public LoginGameMode GameMode;
    [PacketField] public bool IsDebug;
    [PacketField] public bool IsSuperflat;
    [PacketField] public bool HasDeathLocation;

    [PacketOptionalField("HasDeathLocation")]
    public Identifier DeathDimensionName;

    [PacketOptionalField("HasDeathLocation")]
    public Identifier DeathLocation;

    [PacketField] public VarInt PortalCooldown;
}