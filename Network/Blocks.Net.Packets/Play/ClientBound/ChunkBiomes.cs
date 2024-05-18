using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x0E,true,"Play")]
public partial class ChunkBiomes : IPacket
{
    [PacketField] public VarInt NumberOfChunks;
    [PacketArrayField("NumberOfChunks")] public ChunkBiomeData[] BiomeData;
}