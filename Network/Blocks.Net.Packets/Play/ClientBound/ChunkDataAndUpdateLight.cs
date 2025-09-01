using Blocks.Net.Packets.SubPackets;
using Blocks.Net.Packets.SubPackets.Chunks;
using Blocks.Net.PacketSourceGenerator.Attributes;
using LightData = Blocks.Net.Packets.SubPackets.Chunks.LightData;

namespace Blocks.Net.Packets.Play.ClientBound;


// TODO: We should refer to packets by name instead for their ID
// Like here should be "level_chunk_with_light"
// For maintainabilities sake
[Packet("level_chunk_with_light",true, "Play")]
public partial class ChunkDataAndUpdateLight : IPacket
{
    [PacketField] public int ChunkX;
    [PacketField] public int ChunkZ;
    [PacketField] public ChunkData Data;
    [PacketField] public LightData Light;
}