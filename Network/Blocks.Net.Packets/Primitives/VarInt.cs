using System.Runtime.CompilerServices;
using Blocks.Net.Packets.Utilities;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;


[PublicAPI]
public struct VarInt(int v)
{
    private const uint SegmentBits = 0x7f;
    private const uint ContinueBit = 0x80;
    
    public int Value => v;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator int(VarInt v) => v.Value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator VarInt(int v) => new(v);
    
    public void WriteTo(Stream stream)
    {
        var value = (uint)v;
        while (true) {
            if ((value & ~SegmentBits) == 0) {
                stream.WriteByte((byte)value);
                return;
            }

            stream.WriteByte((byte)((value & SegmentBits) | ContinueBit));
            value >>= 7;
        }
    }
    
    
    public static VarInt ReadFrom(Stream stream)
    {
        var value = 0u;
        var position = 0;
        while (true)
        {
            Console.WriteLine("VarInt reading byte");
            var currentByte = stream.CheckedReadByte();
            value |= (currentByte & SegmentBits) << position;

            if ((currentByte & ContinueBit) == 0) break;

            position += 7;

            if (position >= 32) throw new Exception("VarInt is too big");
        }
        return (int)value;
    }
}