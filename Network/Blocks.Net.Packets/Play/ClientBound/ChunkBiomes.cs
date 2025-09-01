using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;
using ChunkBiomeData = Blocks.Net.Packets.SubPackets.Chunks.ChunkBiomeData;

namespace Blocks.Net.Packets.Play.ClientBound;


// How to deal with stuff that depends on the state?
// Should we then add a PacketGlobalState class that gets injected into the arguments
// And then add "RequiresState" attributes?
// Yeah, I think that should work

[Packet("chunks_biomes", true, "Play")]
public partial class ChunkBiomes : IPacket
{
    [PacketField]
    public ChunkBiomeData[] ChunkAndBiomeData;
}