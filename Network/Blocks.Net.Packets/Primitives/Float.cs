using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;

public readonly struct Float(float v)
{
    public float Value => v;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float(Float v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Float(float v) => new(v);

    public static Float ReadFrom(MemoryStream stream)
    {
        var bytes = new byte[4];
        stream.ReadExactly(bytes);
        if (BitConverter.IsLittleEndian)
        {
            bytes = bytes.Reverse().ToArray();
        }
        return BitConverter.ToSingle(bytes);
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