using System.Runtime.CompilerServices;
using Blocks.Net.Packets.Utilities;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;

public readonly struct Int(int v) : IPrimitive
{
    public int Value => v;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator int(Int v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Int(int v) => new(v);

    public static Int ReadFrom(Stream stream)
    {
        var bytes = new byte[4];
        stream.ReadExactly(bytes);
        if (BitConverter.IsLittleEndian)
        {
            bytes = bytes.Reverse().ToArray();
        }
        return BitConverter.ToInt32(bytes);
    }

    public void WriteTo(Stream stream)
    {
        var bytes = BitConverter.GetBytes(v);
        if (BitConverter.IsLittleEndian)
        {
            bytes = bytes.Reverse().ToArray();
        }
        stream.Write(bytes);
    }
}