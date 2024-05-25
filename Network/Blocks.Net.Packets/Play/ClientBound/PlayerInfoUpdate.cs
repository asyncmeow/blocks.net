using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x3C, true, "Play")]
public partial class PlayerInfoUpdate : IPacket
{
    
    [PacketEnum(typeof(byte))] public PlayerAction Actions;
    [PacketField] public VarInt NumberOfPlayers;

    [PacketArrayField("NumberOfPlayers", "CountPlayerActions(Actions)")]
    public PlayerUpdate[] Players;
    
    
    private static int CountPlayerActions(PlayerAction action)
    {
        var count = 0;
        if (action.HasFlag(PlayerAction.AddPlayer)) count += 1;
        if (action.HasFlag(PlayerAction.InitializeChat)) count += 1;
        if (action.HasFlag(PlayerAction.UpdateGameMode)) count += 1;
        if (action.HasFlag(PlayerAction.UpdateListed)) count += 1;
        if (action.HasFlag(PlayerAction.UpdateLatency)) count += 1;
        if (action.HasFlag(PlayerAction.UpdateGameMode)) count += 1;
        return count;
    }
    
}