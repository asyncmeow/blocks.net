using Blocks.Net.DataTypes;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct PotionEffect
{
    [PacketField] public RegistryReference TypeId;
    [PacketField] public PotionDetails Details;
}