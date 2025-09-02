using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("chunk_batch_received",false,"Play")]
public partial class ChunkBatchReceived : IPacket
{
    [PacketField] public float ChunksPerTick;
}