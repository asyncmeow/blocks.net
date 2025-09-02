using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("entity_tag_query",false,"Play")]
public partial class QueryEntityTag : IPacket
{
    [PacketField] public VarInt TransactionId;
    [PacketField] public VarInt EntityId;
}