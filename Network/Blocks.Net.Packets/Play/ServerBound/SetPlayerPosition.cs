using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("move_player_pos",false,"Play")]
public partial class SetPlayerPosition : IPacket
{
    [PacketField] public Double3 Position;
    [PacketField] public byte Flags;
}