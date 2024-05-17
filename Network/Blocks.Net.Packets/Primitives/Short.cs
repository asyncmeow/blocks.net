using System.Runtime.CompilerServices;
using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Packets.Primitives;

public readonly struct Short(short v)
{
    public short Value => v;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator short(Short v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Short(short v) => new(v);


    public void WriteTo(Stream stream)
    {
        // Write in big endian format!
        stream.WriteByte((byte)(Value >> 8));
        stream.WriteByte((byte)(Value & 0xff));
    }

    public static Short ReadFrom(MemoryStream stream)
    {
        var hi = stream.CheckedReadByte();
        var lo = stream.CheckedReadByte();
        var v = (short)((hi << 8) | lo);
        return v;
    }
}