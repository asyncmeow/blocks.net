using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("chunk_batch_finished", true, "Play")]
public partial class ChunkBatchFinished : IPacket
{
    [PacketField] public VarInt BatchSize;
}