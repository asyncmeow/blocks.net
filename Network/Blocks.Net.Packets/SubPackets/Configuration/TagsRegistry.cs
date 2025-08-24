using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Configuration;

[SubPacket]
public partial struct TagsRegistry
{
    [PacketField] public string Registry;
    [PacketField] public VarInt Length;
    [PacketArrayField("Length")] public Tag[] Tags;

}