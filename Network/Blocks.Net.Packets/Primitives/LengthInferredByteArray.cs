using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;

[PublicAPI]
public struct LengthInferredByteArray(byte[] v)
{
    public byte[] Value => v;
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator LengthInferredByteArray(byte[] v) => new(v);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator byte[](LengthInferredByteArray v) => v.Value;


    public static LengthInferredByteArray ReadFrom(MemoryStream stream)
    {
        var length = stream.Length - stream.Position;
        var data = new byte[length];
        stream.ReadExactly(data);
        return data;
    }

    public void WriteTo(MemoryStream stream)
    {
        stream.Write(v);
    }
}