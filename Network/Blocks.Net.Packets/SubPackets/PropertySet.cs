using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct PropertySet
{
    [PacketField] public Identifier Id;
    [PacketField] public ItemId[] Items;
}