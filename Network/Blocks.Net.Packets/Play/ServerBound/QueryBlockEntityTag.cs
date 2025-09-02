using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("block_entity_tag_query",false,"Play")]
public partial class QueryBlockEntityTag : IPacket
{
    [PacketField] public VarInt TransactionId;
    [PacketField] public Position Location;
}