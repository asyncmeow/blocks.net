using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("player_info_remove",true,"Play")]
public partial class PlayerInfoRemove : IPacket
{
    [PacketField] public Uuid[] Uuids;
}