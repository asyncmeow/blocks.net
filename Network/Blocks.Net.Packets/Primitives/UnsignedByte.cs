using System.Runtime.CompilerServices;
using Blocks.Net.Packets.Utilities;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;

public readonly struct UnsignedByte(byte v)
{
    public byte Value => v;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator byte(UnsignedByte v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator UnsignedByte(byte v) => new(v);


    public void WriteTo(Stream stream)
    {
        stream.WriteByte(v);
    }

    public static UnsignedByte ReadFrom(MemoryStream stream)
    {
        return stream.CheckedReadByte();
    }
}