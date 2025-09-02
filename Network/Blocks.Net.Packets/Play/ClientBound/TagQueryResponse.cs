using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("tag_query",true,"Play")]
public partial class TagQueryResponse : IPacket
{
    [PacketField] public VarInt TransactionId;
    [PacketField] public NbtTag Nbt;
}