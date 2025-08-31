using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
[GenerateIdOrXFor]
public partial struct SoundEvent
{
    [PacketField] public Identifier SoundName;
    [PacketField] public float? Range;
}