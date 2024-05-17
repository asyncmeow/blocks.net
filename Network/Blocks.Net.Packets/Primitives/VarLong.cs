using System.Runtime.CompilerServices;
using Blocks.Net.Packets.Utilities;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;

public readonly struct VarLong(long v)
{
    private const ulong SegmentBits = 0x7f;
    private const ulong ContinueBit = 0x80;
    public long Value => v;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator long(VarLong v) => v.Value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator VarLong(long v) => new(v);
    
    public void WriteTo(Stream stream)
    {
        var value = (ulong)v;
        while (true) {
            if ((value & ~SegmentBits) == 0) {
                stream.WriteByte((byte)value);
                return;
            }
            stream.WriteByte((byte)((value & SegmentBits) | ContinueBit));
            value >>= 7;
        }
    }
    
    
    public static VarLong ReadFrom(MemoryStream stream)
    {
        var value = 0UL;
        var position = 0;
        while (true)
        {
            var currentByte = stream.CheckedReadByte();
            value |= (currentByte & SegmentBits) << position;

            if ((currentByte & ContinueBit) == 0) break;

            position += 7;

            if (position >= 64) throw new Exception("VarLong is too big");
        }
        return (long)value;
    }
}