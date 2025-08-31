using System.Runtime.CompilerServices;
using Blocks.Net.Nbt;
using Blocks.Net.Packets.Utilities;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;

public readonly struct Nbt(NbtTag v) : IPrimitive
{
    public NbtTag Value => v;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator NbtTag(Nbt v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Nbt(NbtTag v) => new(v);


    public static Nbt ReadFrom(Stream stream, PacketState state) => NbtTag.Read(stream, false);

    public void WriteTo(Stream stream, PacketState state)
    {
        v.Write(stream, false);
    }
}