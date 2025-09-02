using System.Numerics;
using System.Runtime.CompilerServices;
using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("player_info_update",true,"Play")]
public partial class PlayerInfoUpdate : IPacket
{
    [PacketEnum(typeof(byte))] public PlayerAction Actions;
    [PacketField("ActionsCount(Actions)")] public PlayerUpdate[] Updates;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int ActionsCount(PlayerAction actions)
    {
        return BitOperations.PopCount((uint)actions);
    }
}