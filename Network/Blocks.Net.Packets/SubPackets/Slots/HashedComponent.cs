using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots;

[SubPacket]
public partial struct HashedComponent
{
    [PacketEnum(typeof(VarInt))] public StructuredComponent ComponentType;
    [PacketField] public int CheckSum;
}