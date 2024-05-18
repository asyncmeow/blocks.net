using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct SoundEffect
{
    [PacketField] public Identifier SoundName;
    [PacketField] public bool HasFixedRange;
    [PacketOptionalField("HasFixedRange")] public float Range;
}