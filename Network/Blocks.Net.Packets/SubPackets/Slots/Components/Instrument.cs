using Blocks.Net.Nbt;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
[GenerateIdOrXFor]
public partial struct Instrument
{
    [PacketField] public IdOrSoundEvent SoundEvent;
    [PacketField] public float SoundRange;
    [PacketField] public float Range;
    [PacketField] public NbtTag Description;
}