using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial class PotionDetails
{
    [PacketField] public VarInt Amplifier;
    [PacketField] public VarInt Duration;
    [PacketField] public bool Ambient;
    [PacketField] public bool ShowParticles;
    [PacketField] public bool ShowIcon;
    [PacketField] public PotionDetails? HiddenEffect;
}