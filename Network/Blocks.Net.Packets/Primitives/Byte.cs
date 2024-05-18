using System.Runtime.CompilerServices;
using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Packets.Primitives;

public readonly struct Byte(sbyte v)
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

    public static Byte ReadFrom(MemoryStream stream)
    {
        return (sbyte)stream.CheckedReadByte();
    }
}