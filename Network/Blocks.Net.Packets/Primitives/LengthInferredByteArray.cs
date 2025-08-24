using System.Runtime.CompilerServices;
using Blocks.Net.Packets.Utilities;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;

public readonly struct LengthInferredByteArray(byte[] v) : IPrimitive
{
    public byte[] Value => v;
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator LengthInferredByteArray(byte[] v) => new(v);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator byte[](LengthInferredByteArray v) => v.Value;


    public static LengthInferredByteArray ReadFrom(Stream stream)
    {
        var length = stream.Length - stream.Position;
        var data = new byte[length];
        stream.ReadExactly(data);
        return data;
    }

    public void WriteTo(Stream stream)
    {
        stream.Write(v);
    }
}