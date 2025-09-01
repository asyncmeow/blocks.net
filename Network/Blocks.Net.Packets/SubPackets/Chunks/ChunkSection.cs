using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Chunks;

[SubPacket]
[RequiresStateField(typeof(byte),"BiomeMinBitsPerEntry")]
public partial struct ChunkSection
{
    [PacketField] public short BlockCount;
    [PacketField("4096", "8", "15")] public PalettedContainer BlockStates;
    [PacketField("64", "3", "state.BiomeMinBitsPerEntry")] public PalettedContainer Biomes;
}