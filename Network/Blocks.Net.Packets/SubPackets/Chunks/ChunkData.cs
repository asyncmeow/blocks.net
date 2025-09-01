using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Chunks;

[SubPacket]
public partial struct ChunkData
{
    [PacketField] public Heightmap[] Heightmaps;
    [PacketField] public ChunkDataArray Data;
    [PacketField] public BlockEntity[] BlockEntities;
}