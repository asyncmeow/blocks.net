using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.EntityMetadata;

[SubPacket]
[GenerateIdOrXFor]
public partial struct PaintingVariant
{
    [PacketField] public int Width;
    [PacketField] public int Height;
    [PacketField] public Identifier AssetId;
    [PacketField] public NbtTag? Title;
    [PacketField] public NbtTag? Author;
}