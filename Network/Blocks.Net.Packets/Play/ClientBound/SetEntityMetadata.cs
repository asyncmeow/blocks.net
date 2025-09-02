using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.EntityMetadata;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_entity_data",true,"Play")]
public partial class SetEntityMetadata : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public EntityMetadata Metadata;
}