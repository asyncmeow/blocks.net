using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("respawn",true,"Play")]
public partial class Respawn : IPacket
{
    [PacketField] public RegistryReference DimensionType;
    [PacketField] public Identifier DimensionName;
    [PacketField] public long HashedSeed;
    [PacketField] public byte GameMode;
    [PacketField] public sbyte PreviousGameMode;
    [PacketField] public bool IsDebug;
    [PacketField] public DeathLocationInformation? DeathLocation;
    [PacketField] public VarInt PortalCooldown;
    [PacketField] public VarInt SeaLevel;
    [PacketField] public byte DataKept;
    
}