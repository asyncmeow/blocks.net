using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Configuration;

[SubPacket]
public partial struct TagsRegistry
{
    [PacketField] public Identifier Registry;
    [PacketField] public Tag[] Tags;
}