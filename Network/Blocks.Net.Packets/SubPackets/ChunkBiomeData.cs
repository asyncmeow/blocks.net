using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct ChunkBiomeData
{
    [PacketField] public int ChunkZ;
    [PacketField] public int ChunkX;
    [PacketField] public VarInt Size;
    [PacketArrayField("Size")] public byte[] Data;
}