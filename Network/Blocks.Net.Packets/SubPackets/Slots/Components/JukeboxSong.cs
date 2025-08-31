using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
[GenerateIdOrXFor]
public partial struct JukeboxSong
{
    [PacketField] public IdOrSoundEvent SoundEvent;
    [PacketField] public NbtTag Description;
    [PacketField] public float Duration;
    [PacketField] public VarInt Output;
}