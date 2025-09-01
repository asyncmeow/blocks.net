using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;
using TagsRegistry = Blocks.Net.Packets.SubPackets.Configuration.TagsRegistry;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet("update_tags",true,"Configuration")]
public partial class UpdateTags : IPacket
{
    [PacketField] public TagsRegistry[] Registries;
}