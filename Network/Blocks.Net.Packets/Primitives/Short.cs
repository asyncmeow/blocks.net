using System.Runtime.CompilerServices;
using Blocks.Net.Packets.Utilities;
using StreamUtilities = Blocks.Net.Nbt.Utilities.StreamUtilities;

namespace Blocks.Net.Packets.Primitives;

public readonly struct Short(short v) : IPrimitive
{
    public short Value => v;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator short(Short v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Short(short v) => new(v);


    public void WriteTo(Stream stream, PacketState state)
    {
        // Write in big endian format!
        stream.WriteByte((byte)(Value >> 8));
        stream.WriteByte((byte)(Value & 0xff));
    }

    public static Short ReadFrom(Stream stream, PacketState state)
    {
        var hi = StreamUtilities.CheckedReadByte(stream);
        var lo = StreamUtilities.CheckedReadByte(stream);
        var v = (short)((hi << 8) | lo);
        return v;
    }
}