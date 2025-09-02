using Blocks.Net.Packets.Enums;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.EntityMetadata;

[SubPacket]
public partial struct EntityMetadata
{
    [PacketField] public byte Index;
    [PacketField] public EntityDataValueImpl Value;
}