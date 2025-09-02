using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct StonecutterRecipe
{
    [PacketField] public IdSet Ingredients;
    [PacketField] public SlotDisplayImpl Display;
}