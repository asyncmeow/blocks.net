using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct Bee
{
    [PacketField] public NbtTag EntityData;
    [PacketField] public VarInt TicksInHive;
    [PacketField] public VarInt MinTicksInHive;
}