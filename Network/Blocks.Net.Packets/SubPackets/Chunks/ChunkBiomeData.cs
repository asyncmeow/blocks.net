using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Chunks;

[SubPacket]
public partial struct ChunkBiomeData
{
    [PacketField] public int ChunkZ;
    [PacketField] public int ChunkX;
    [PacketField] public ChunkBiomeArray[] Data;
}