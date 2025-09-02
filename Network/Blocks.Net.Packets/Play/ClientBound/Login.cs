using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("login",true,"Play")]
public partial class Login : IPacket
{
    [PacketField] public int EntityId;
    [PacketField] public bool IsHardcore;
    [PacketField] public Identifier[] DimensionNames;
    [PacketField] public VarInt MaxPlayers;
    [PacketField] public VarInt ViewDistance;
    [PacketField] public VarInt SimulationDistance;
    [PacketField] public bool ReducedDebugInfo;
    [PacketField] public bool EnableRespawnScreen;
    [PacketField] public bool DoLimitedCrafting;
    [PacketField] public RegistryReference DimensionType;
    [PacketField] public Identifier DimensionName;
    [PacketField] public long HashedSeed;
    [PacketField] public byte GameMode;
    [PacketField] public sbyte PreviousGameMode;
    [PacketField] public bool IsDebug;
    [PacketField] public bool IsFlat;
    [PacketField] public DeathLocationInformation? DeathLocation;
    [PacketField] public VarInt PortalCooldown;
    [PacketField] public VarInt SeaLevel;
    [PacketField] public bool EnforcesSecureChat;
}