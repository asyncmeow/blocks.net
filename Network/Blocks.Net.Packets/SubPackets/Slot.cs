using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct Slot
{
    [PacketField] public bool Present;
    [PacketOptionalField("Present")] public VarInt ItemId;
    [PacketOptionalField("Present")] public sbyte ItemCount;
    [PacketOptionalField("Present")] public NbtTag Nbt;
}