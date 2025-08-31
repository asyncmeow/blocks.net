using System.Runtime.CompilerServices;
using Blocks.Net.DataTypes;
using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Utilities;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;


public readonly struct VarInt(int v) : IPrimitive
{
    private const uint SegmentBits = 0x7f;
    private const uint ContinueBit = 0x80;
    
    public int Value => v;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator int(VarInt v) => v.Value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator VarInt(int v) => new(v);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator RegistryReference(VarInt v) => new(v.Value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator VarInt(RegistryReference v) => new(v.RegistryId);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator BlockState(VarInt v) => new(v.Value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator VarInt(BlockState v) => new(v.StateId);
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator ItemId(VarInt v) => new(v.Value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator VarInt(ItemId v) => new(v.Id);

    
    public void WriteTo(Stream stream, PacketState state)
    {
        var value = (uint)v;
        while (true) {
            if ((value & ~SegmentBits) == 0) {
                stream.WriteByte((byte)value);
                return;
            }

            stream.WriteByte((byte)((value & SegmentBits) | ContinueBit));
            value >>= 7;
        }
    }
    
    
    public static VarInt ReadFrom(Stream stream, PacketState state)
    {
        var value = 0u;
        var position = 0;
        while (true)
        {
            var currentByte = stream.CheckedReadByte();
            value |= (currentByte & SegmentBits) << position;

            if ((currentByte & ContinueBit) == 0) break;

            position += 7;

            if (position >= 32) throw new Exception("VarInt is too big");
        }
        return (int)value;
    }
}