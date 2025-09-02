using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("update_tags",true,"Play")]
public partial class UpdateTags : IPacket
{
    [PacketField] public Identifier Registry;
    [PacketField] public Tag[] Tags;
}