using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct Enchantment
{
    [PacketField] public RegistryReference TypeId;
    [PacketField] public VarInt Level;
}