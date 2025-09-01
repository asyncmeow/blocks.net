using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("chunk_batch_start",true,"Play")]
public partial class ChunkBatchStart : IPacket;