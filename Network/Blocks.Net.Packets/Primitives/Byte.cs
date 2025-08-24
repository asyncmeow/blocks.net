using System.Runtime.CompilerServices;
using Blocks.Net.Packets.Utilities;
using StreamUtilities = Blocks.Net.Nbt.Utilities.StreamUtilities;

namespace Blocks.Net.Packets.Primitives;

public readonly struct Byte(sbyte v) : IPrimitive
{
    public sbyte Value => v;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator sbyte(Byte v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Byte(sbyte v) => new(v);


    public void WriteTo(Stream stream)
    {
        stream.WriteByte((byte)v);
    }

    public static Byte ReadFrom(Stream stream)
    {
        return (sbyte)StreamUtilities.CheckedReadByte(stream);
    }
}