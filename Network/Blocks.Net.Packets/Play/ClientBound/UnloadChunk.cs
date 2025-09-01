using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("forget_level_chunk ", true, "Play")]
public partial class UnloadChunk : IPacket
{
    [PacketField] public int ChunkZ;
    [PacketField] public int ChunkX;
}