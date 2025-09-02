using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_entity_link",true,"Play")]
public partial class LinkEntities : IPacket
{
    [PacketField] public int AttachingEntityId;
    [PacketField] public int HoldingEntityId;
}