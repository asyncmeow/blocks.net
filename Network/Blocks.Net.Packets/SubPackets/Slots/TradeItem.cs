using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots;

[SubPacket]
public partial struct TradeItem
{
    [PacketField] public RegistryReference ItemId;
    [PacketField] public VarInt ItemCount;
    [PacketField] public StructuredComponentImpl[] Components;
}