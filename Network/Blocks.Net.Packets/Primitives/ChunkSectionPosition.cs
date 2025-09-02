using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Primitives;

[SubPacket]
public partial struct ChunkSectionPosition
{
    [PacketField] private long _writtenData;

    private ulong _data
    {
        get => (ulong)_writtenData;
        set => _writtenData = (long)value;
    }
    // Format
    // [X:22][Z:22][Y:20]

    public uint X
    {
        get => (uint)(_data >> 42);
        set => _data = (_data & ~(0x3FFFFFUL << 42)) | ((ulong)value << 42);
    }
    public uint Z
    {
        get => (uint)(_data >> 20) & 0x3FFFFF;
        set => _data = (_data & ~(0x3FFFFFUL << 20)) | ((ulong)value << 20);
    }

    public uint Y
    {
        get => (uint)(_data & 0xFFFFF);
        set => _data = (_data & ~0xFFFFFUL) | value;
    }
}