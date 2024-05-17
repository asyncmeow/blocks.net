using System.Runtime.CompilerServices;
using Blocks.Net.Nbt;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;

public readonly struct Nbt(NbtTag v)
{
    public NbtTag Value => v;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator NbtTag(Nbt v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Nbt(NbtTag v) => new(v);


    public static Nbt ReadFrom(MemoryStream stream) => NbtTag.Read(stream, false);

    public void WriteTo(Stream stream)
    {
        v.Write(stream, false);
    }
}