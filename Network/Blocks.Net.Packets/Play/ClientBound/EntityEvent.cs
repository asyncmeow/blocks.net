using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x1D,true,"Play")]
public partial class EntityEvent : IPacket
{
    [PacketField] public int EntityId;
    [PacketField] public sbyte EntityStatus;
}