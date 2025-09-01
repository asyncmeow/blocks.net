using Blocks.Net.Packets.Primitives;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.SubPackets;

[PublicAPI]
public struct LongPackedDataArray(int[] data)
{
    public int[] Data => data;
    public ref int this[int index] => ref Data[index];
    
    public void WriteTo(Stream stream, PacketState state, int length, byte bitsPerEntry)
    {
        // Let's make a thread local buffer for this
        var entriesPerLong = 64 / bitsPerEntry;
        var mask = ~(0xFFFFFFFFFFFFFFFF << bitsPerEntry);
        for (var i = 0; i < length; i++)
        {
            if (length - i < entriesPerLong)
            {
                entriesPerLong = length - i;
            }

            long beingConstructed = 0;
            for (var j = entriesPerLong - 1; j >= 0; j--)
            {
                beingConstructed |= (long)((uint)Data[i+j] & mask);
                beingConstructed <<= length;
            }
            
            new Long(beingConstructed).WriteTo(stream, state);
        }
    }

    public static LongPackedDataArray ReadFrom(Stream stream, PacketState state, int length, byte bitsPerEntry)
    {
        var entriesPerLong = 64 / bitsPerEntry;
        var mask = ~(0xFFFFFFFFFFFFFFFF << bitsPerEntry);
        var result = new int[length];
        for (var i = 0; i < length; i+= entriesPerLong)
        {
            long packedEntries = Long.ReadFrom(stream, state);
            for (var j = 0; j < entriesPerLong && (i + j) < length; j += 1)
            {
                result[i + j] = (int)(packedEntries & (long)mask);
                packedEntries >>= bitsPerEntry;
            }
        }
        return new LongPackedDataArray(result);
    }
    

    public static LongPackedDataArray ReadFrom(Stream stream, PacketState state, int length, byte bitsPerEntry, Func<int,int> transformer)
    {
        var entriesPerLong = 64 / bitsPerEntry;
        var mask = ~(0xFFFFFFFFFFFFFFFF << bitsPerEntry);
        var result = new int[length];
        for (var i = 0; i < length; i+= entriesPerLong)
        {
            long packedEntries = Long.ReadFrom(stream, state);
            for (var j = 0; j < entriesPerLong && (i + j) < length; j += 1)
            {
                result[i + j] = transformer((int)(packedEntries & (long)mask));
                packedEntries >>= bitsPerEntry;
            }
        }
        return new LongPackedDataArray(result);
    }
}