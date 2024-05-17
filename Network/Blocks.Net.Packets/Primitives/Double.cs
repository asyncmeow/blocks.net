using System.Runtime.CompilerServices;

namespace Blocks.Net.Packets.Primitives;

public readonly struct Double(double v)
{
    public double Value => v;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator double(Double v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Double(double v) => new(v);

    public static Double ReadFrom(MemoryStream stream)
    {
        var bytes = new byte[8];
        stream.ReadExactly(bytes);
        if (BitConverter.IsLittleEndian)
        {
            bytes = bytes.Reverse().ToArray();
        }
        return BitConverter.ToDouble(bytes);
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