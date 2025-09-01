using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Chunks;

[SubPacket]
[RequiresStateField(typeof(byte),"BiomeMinBitsPerEntry")]
public partial struct ChunkBiomeSection
{
    [PacketField] public short BlockCount;
    [PacketField("64", "3", "state.BiomeMinBitsPerEntry")] public PalettedContainer Biomes;
}