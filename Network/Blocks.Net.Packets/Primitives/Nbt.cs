using System.Runtime.CompilerServices;
using Blocks.Net.Nbt;
using Blocks.Net.Packets.Utilities;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;

[GenerateIdOrXFor]
public readonly struct Nbt(NbtTag v) : IPrimitive
{
    public NbtTag Value => v;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator NbtTag(Nbt v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Nbt(NbtTag v) => new(v);
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Nbt(TextComponent v) => v.ToNbt();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator TextComponent(Nbt v) => TextComponent.FromNbt(v);


    public static Nbt ReadFrom(Stream stream, PacketState state) => NbtTag.Read(stream, false);

    public void WriteTo(Stream stream, PacketState state)
    {
        v.Write(stream, false);
    }
}