using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Primitives;

[SubPacket]
public partial struct BitSet
{
    [PacketField] public int Length = 0;
    [PacketArrayField("Length")] public long[] Data = [];

    public BitSet()
    {
    }

    public BitSet(int size)
    {
        Length = size;
        Data = new long[size];
    }
    
    public bool this[int index]
    {
        get => (Data[index / 64] & (1L << (index % 64))) != 0;
        set
        {
            if (value)
            {
                Data[index / 64] |= 1L << (index % 64);
            }
            else
            {
                Data[index / 64] &= ~(1L << (index % 64));
            }
        }
    }

    public void Resize(int newLength)
    {
        Length = newLength;
        var newArray = new long[newLength];
        Data.CopyTo(newArray.AsSpan());
        Data = newArray;
    }
}