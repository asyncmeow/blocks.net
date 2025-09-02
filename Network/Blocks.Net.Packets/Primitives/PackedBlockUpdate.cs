using Blocks.Net.DataTypes;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Primitives;

[SubPacket]
public partial struct PackedBlockUpdate
{
    [PacketField] private VarLong _data;
    
    // Encoded as (ID << 12)[X:4,Y:4,Z:4]

    public BlockState State
    {
        get => new((int)(_data >> 12));
        set => _data = (_data & 0xFFF) | ((long)value.StateId << 12);
    }

    public byte X
    {
        get => (byte)((_data >> 8) & 0xF);
        set => _data = (_data & ~0xF00L) | ((long)value << 8);
    }

    public byte Z
    {
        get => (byte)((_data >> 4) & 0xF);
        set => _data = (_data & ~0xF0L) | ((long)value << 4);
    }
    
    
    public byte Y
    {
        get => (byte)(_data & 0xF);
        set => _data = (_data & ~0xFL) | value;
    }
}