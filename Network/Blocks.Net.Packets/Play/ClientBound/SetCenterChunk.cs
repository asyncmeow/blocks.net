using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_chunk_cache_center",true,"Play")]
public partial class SetCenterChunk : IPacket
{
    [PacketField] public VarInt ChunkX;
    [PacketField] public VarInt ChunkZ;
}