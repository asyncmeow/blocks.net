using Blocks.Net.Nbt;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
[GenerateIdOrXFor]
public partial struct TrimMaterial
{
    [PacketField] public string Suffix;
    [PacketField] public TrimOverride[] Overrides;
    [PacketField] public NbtTag Description;
}