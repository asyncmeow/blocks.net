using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;

public readonly struct Uuid(Guid v)
{
    public Guid Value => v;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Guid(Uuid v) => v.Value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Uuid(Guid v) => new(v);

    public static Uuid ReadFrom(MemoryStream stream)
    {
        var bytes = new byte[16];
        stream.ReadExactly(bytes);
        // Do some bit fanangling cuz guids are stored oddly (first 3 parts are stored LE, and last 2 parts BE)
        if (BitConverter.IsLittleEndian)
        {
            // Flip arround the first 4 parts
            var a3 = bytes[0];
            var a2 = bytes[1];
            var a1 = bytes[2];
            var a0 = bytes[3];
            bytes[0] = a0;
            bytes[1] = a1;
            bytes[2] = a2;
            bytes[3] = a3;

            var b1 = bytes[4];
            var b0 = bytes[5];
            bytes[4] = b0;
            bytes[5] = b1;

            var c1 = bytes[6];
            var c0 = bytes[7];
            bytes[6] = c0;
            bytes[7] = c1;
        }
        return new Guid(bytes);
    }

    public void WriteTo(Stream stream)
    {
        var bytes = v.ToByteArray();
        // Do some bit fanangling cuz guids are stored oddly (first 3 parts are stored LE, and last 2 parts BE)
        if (BitConverter.IsLittleEndian)
        {
            var a3 = bytes[0];
            var a2 = bytes[1];
            var a1 = bytes[2];
            var a0 = bytes[3];
            bytes[0] = a0;
            bytes[1] = a1;
            bytes[2] = a2;
            bytes[3] = a3;

            var b1 = bytes[4];
            var b0 = bytes[5];
            bytes[4] = b0;
            bytes[5] = b1;

            var c1 = bytes[6];
            var c0 = bytes[7];
            bytes[6] = c0;
            bytes[7] = c1;
        }
        stream.Write(bytes);
    }
}