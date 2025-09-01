using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("entity_event",true,"Play")]
public partial class EntityEvent : IPacket
{
    [PacketField] public int EntityId;
    [PacketField] public byte EntityStatus;
}