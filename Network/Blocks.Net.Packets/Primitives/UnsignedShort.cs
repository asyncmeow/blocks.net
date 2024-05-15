using System.Runtime.CompilerServices;
using Blocks.Net.Packets.Utilities;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;

[PublicAPI]
public struct UnsignedShort(ushort v)
{
    public ushort Value => v;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ushort(UnsignedShort v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator UnsignedShort(ushort v) => new(v);


    public void WriteTo(Stream stream)
    {
        // Write in big endian format!
        stream.WriteByte((byte)(Value >> 8));
        stream.WriteByte((byte)(Value & 0xff));
    }

    public static UnsignedShort ReadFrom(MemoryStream stream)
    {
        var hi = stream.CheckedReadByte();
        var lo = stream.CheckedReadByte();
        var v = (ushort)((hi << 8) | lo);
        return v;
    }
}