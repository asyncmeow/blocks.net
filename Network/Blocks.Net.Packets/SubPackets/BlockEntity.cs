using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct BlockEntity
{
    [PacketField] public byte PackedXZ;

    public byte X
    {
        get => (byte)(PackedXZ >> 4);
        set => PackedXZ = (byte)((PackedXZ & 0x0F) | (value << 4));
    }

    public byte Z
    {
        get => (byte)(PackedXZ & 0x0F);
        set => PackedXZ = (byte)((PackedXZ & 0xF0) | (value & 0x0F));
    }

    [PacketField] public short Y;
    [PacketField] public VarInt Type;
    [PacketField] public NbtTag Data;
}