using Blocks.Net.Packets.Enums;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_player_team",true,"Play")]
public partial class UpdateTeams : IPacket
{
    [PacketField] public string TeamName;
    [PacketField] public TeamUpdateMethodImpl Method;
}