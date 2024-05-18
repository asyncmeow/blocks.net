using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x0D,true,"Play")]
public partial class ChunkBatchStart : IPacket;