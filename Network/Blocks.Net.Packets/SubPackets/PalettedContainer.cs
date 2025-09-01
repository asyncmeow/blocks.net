using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.Utilities;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.SubPackets;

/// <summary>
/// Implements the paletted API in the Chunk Data Structure
/// </summary>
[PublicAPI]
public readonly struct PalettedContainer(int[] dataArray)
{
    public readonly int[] DataArray = dataArray;

    private struct PalettedContainerSubContainer
    {
        public readonly byte BitsPerEntry;
        public readonly int[] Palette;
        public readonly int[] ActualData;

        public PalettedContainerSubContainer(int[] data, int maxBitsPerEntryIndirect, int bitsPerEntryDirect)
        {
            // All the states can be determined by one thing
            var allDistinct = data.Distinct().ToArray();
            var numUnique = allDistinct.Length;
            var bitsPerNumUnique = (byte)Math.Ceiling(Math.Log2(numUnique));
            if (bitsPerNumUnique == 0)
            {
                BitsPerEntry = 0;
                Palette = [data[0]];
                ActualData = [];
            }
            else if (bitsPerNumUnique <= maxBitsPerEntryIndirect)
            {
                var reverseLookup = new Dictionary<int, int>();
                for (var i = 0; i < allDistinct.Length; i++)
                {
                    reverseLookup[allDistinct[i]] = i;
                }

                BitsPerEntry = bitsPerNumUnique;
                Palette = allDistinct;
                ActualData = data.Select(x => reverseLookup[x]).ToArray();
            }
            else
            {
                BitsPerEntry = (byte)bitsPerEntryDirect;
                Palette = [];
                ActualData = data;
            }
        }

        public PalettedContainerSubContainer(byte bitsPerEntry, int[] palette, int[] dataArray)
        {
            BitsPerEntry = bitsPerEntry;
            Palette = palette;
            ActualData = dataArray;
        }


        public void WriteTo(Stream stream, PacketState state)
        {
            stream.WriteByte(BitsPerEntry);
            if (BitsPerEntry == 0)
            {
                new VarInt(Palette[0]).WriteTo(stream, state);
            }
            else if (Palette.Length > 0)
            {
                new VarInt(Palette.Length).WriteTo(stream, state);
                foreach (var entry in Palette)
                {
                    new VarInt(entry).WriteTo(stream, state);
                }
            }

            if (BitsPerEntry == 0) return;

            new LongPackedDataArray(ActualData).WriteTo(stream, state, ActualData.Length, BitsPerEntry);
        }

        public static int[] ReadDataFrom(Stream stream, PacketState state, int length, byte maxBitsPerEntryIndirect)
        {
            var bitsPerEntry = stream.CheckedReadByte();
            if (bitsPerEntry == 0)
            {
                var result = new int[length];
                Array.Fill(result, VarInt.ReadFrom(stream, state));
                return result;
            }


            if (bitsPerEntry <= maxBitsPerEntryIndirect)
            {
                var palette = new int[VarInt.ReadFrom(stream, state)];
                for (var i = 0; i < palette.Length; i++)
                {
                    palette[i] = VarInt.ReadFrom(stream, state);
                }

                var dataArray = LongPackedDataArray.ReadFrom(stream, state, length, bitsPerEntry, x => palette[x]);
                return dataArray.Data;
            }
            else
            {
                var dataArray = LongPackedDataArray.ReadFrom(stream, state, length, bitsPerEntry);
                return dataArray.Data;
            }
        }
    }


    public void WriteTo(Stream stream, PacketState state, int length, byte maxBitsPerEntryIndirect,
        byte bitsPerEntryDirect)
    {
        var subContainer = new PalettedContainerSubContainer(DataArray, maxBitsPerEntryIndirect, bitsPerEntryDirect);
        subContainer.WriteTo(stream, state);
    }

    public static PalettedContainer ReadFrom(Stream stream, PacketState state, int length, byte maxBitsPerEntryIndirect,
        byte bitsPerEntryDirect)
    {
        return new(PalettedContainerSubContainer.ReadDataFrom(stream, state, length, bitsPerEntryDirect));
    }
}