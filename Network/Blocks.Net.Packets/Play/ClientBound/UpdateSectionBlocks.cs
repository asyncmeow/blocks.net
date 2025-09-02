using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("section_blocks_update",true,"Play")]
public partial class UpdateSectionBlocks : IPacket
{
    [PacketField] public ChunkSectionPosition SectionPosition;
    [PacketField] public PackedBlockUpdate[] BlockUpdates;
}