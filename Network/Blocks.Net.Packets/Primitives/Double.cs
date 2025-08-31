using System.Runtime.CompilerServices;
using Blocks.Net.Packets.Utilities;

namespace Blocks.Net.Packets.Primitives;

public readonly struct Double(double v) : IPrimitive
{
    public double Value => v;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator double(Double v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Double(double v) => new(v);

    public static Double ReadFrom(Stream stream, PacketState state)
    {
        var bytes = new byte[8];
        stream.ReadExactly(bytes);
        if (BitConverter.IsLittleEndian)
        {
            bytes = bytes.Reverse().ToArray();
        }
        return BitConverter.ToDouble(bytes);
    }

    public void WriteTo(Stream stream, PacketState state)
    {
        var bytes = BitConverter.GetBytes(v);
        if (BitConverter.IsLittleEndian)
        {
            bytes = bytes.Reverse().ToArray();
        }
        stream.Write(bytes);
    }
}