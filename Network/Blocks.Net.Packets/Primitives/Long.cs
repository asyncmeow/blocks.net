using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;


[PublicAPI]
public struct Long(long v)
{
    public long Value => v;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator long(Long v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Long(long v) => new(v);

    public static Long ReadFrom(MemoryStream stream)
    {
        var bytes = new byte[8];
        stream.ReadExactly(bytes);
        if (BitConverter.IsLittleEndian)
        {
            bytes = bytes.Reverse().ToArray();
        }
        return BitConverter.ToInt64(bytes);
    }

    public void WriteTo(MemoryStream stream)
    {
        var bytes = BitConverter.GetBytes(v);
        if (BitConverter.IsLittleEndian)
        {
            bytes = bytes.Reverse().ToArray();
        }
        stream.Write(bytes);
    }
}