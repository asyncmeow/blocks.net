using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet(0x09,true,"Configuration")]
public partial class UpdateTags : IPacket
{
    [PacketField] public VarInt TagsCount;
    [PacketArrayField("TagsCount")] public TagsRegistry[] Registries;
}