using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct Tag
{
    [PacketField] public string TagName;
    [PacketField] public VarInt Count;
    [PacketArrayField("Count")] public VarInt[] Entries;
}