using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct SuspiciousStewEffect
{
    [PacketField] public RegistryReference TypeId;
    [PacketField] public VarInt Duration;
}