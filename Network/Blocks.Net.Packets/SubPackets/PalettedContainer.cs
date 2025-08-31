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
        public readonly ulong[] DataArray;

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
                DataArray = [];
            }
            else if (bitsPerNumUnique <= maxBitsPerEntryIndirect)
            {
                var reverseLookup = new Dictionary<int, ulong>();
                for (var i = 0; i < allDistinct.Length; i++)
                {
                    reverseLookup[allDistinct[i]] = (ulong)i;
                }

                BitsPerEntry = bitsPerNumUnique;
                Palette = allDistinct;
                var entriesPerLong = 64 / bitsPerNumUnique;
                DataArray = data.Chunk(entriesPerLong).Select(entries => entries.Reverse().Aggregate(0UL,
                    (previous, current) => (previous << bitsPerEntryDirect) | reverseLookup[current])).ToArray();
            }
            else
            {
                BitsPerEntry = (byte)bitsPerEntryDirect;
                Palette = [];
                var entriesPerLong = 64 / maxBitsPerEntryIndirect;
                DataArray = data.Chunk(entriesPerLong).Select(entries =>
                        entries.Reverse().Aggregate(0UL,
                            (previous, current) => (previous << bitsPerEntryDirect) | (uint)current))
                    .ToArray();
            }
        }

        public PalettedContainerSubContainer(byte bitsPerEntry, int[] palette, ulong[] dataArray)
        {
            BitsPerEntry = bitsPerEntry;
            Palette = palette;
            DataArray = dataArray;
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

            foreach (var entry in DataArray)
            {
                new Long((long)entry).WriteTo(stream, state);
            }
        }

        public static PalettedContainerSubContainer ReadFrom(Stream stream, PacketState state, int length, byte maxBitsPerEntryIndirect)
        {
            var bitsPerEntry = stream.CheckedReadByte();
            if (bitsPerEntry == 0)
            {
                int[] palette = [VarInt.ReadFrom(stream,state)];
                return new PalettedContainerSubContainer(bitsPerEntry, palette, []);
            }

            if (bitsPerEntry <= maxBitsPerEntryIndirect)
            {
                var palette = new int[VarInt.ReadFrom(stream,state)];
                for (var i = 0; i < palette.Length; i++)
                {
                    palette[i] = VarInt.ReadFrom(stream,state);
                }

                var entriesPerLong = 64 / bitsPerEntry;
                var size = (int)Math.Ceiling(length / (float)entriesPerLong);
                var dataArray = new ulong[size];
                for (var i = 0; i < dataArray.Length; i++)
                {
                    dataArray[i] = (ulong)(long)Long.ReadFrom(stream,state);
                }

                return new PalettedContainerSubContainer(bitsPerEntry, palette, dataArray);
            }
            else
            {
                var entriesPerLong = 64 / bitsPerEntry;
                var size = (int)Math.Ceiling(length / (float)entriesPerLong);
                var dataArray = new ulong[size];
                for (var i = 0; i < dataArray.Length; i++)
                {
                    dataArray[i] = (ulong)(long)Long.ReadFrom(stream,state);
                }

                return new PalettedContainerSubContainer(bitsPerEntry, [], dataArray);
            }
        }

        public int[] GetData(int length)
        {
            var data = new int[length];
            if (BitsPerEntry == 0)
            {
                Array.Fill(data, Palette[0]);
            }
            else if (Palette.Length > 0)
            {
                var currentBit = 0;
                var currentDataIndex = 0;
                var currentData = DataArray[currentDataIndex];
                var mask = ~(0xFFFFFFFFFFFFFFFF << BitsPerEntry);
                for (var i = 0; i < length; i++)
                {
                    if (currentBit + BitsPerEntry > 64)
                    {
                        currentBit = 0;
                        currentData = DataArray[++currentDataIndex];
                    }

                    var subData = (currentData >> currentBit) & mask;
                    data[i] = Palette[subData];
                    currentBit += BitsPerEntry;
                }
            }
            else
            {
                var currentBit = 0;
                var currentDataIndex = 0;
                var currentData = DataArray[currentDataIndex];
                var mask = ~(0xFFFFFFFFFFFFFFFF << BitsPerEntry);
                for (var i = 0; i < length; i++)
                {
                    if (currentBit + BitsPerEntry > 64)
                    {
                        currentBit = 0;
                        currentData = DataArray[++currentDataIndex];
                    }

                    var subData = (currentData >> currentBit) & mask;
                    data[i] = (int)subData;
                    currentBit += BitsPerEntry;
                }
            }

            return data;
        }
    }


    public void WriteTo(Stream stream, PacketState state, int length, byte maxBitsPerEntryIndirect, byte bitsPerEntryDirect)
    {
        var subContainer = new PalettedContainerSubContainer(DataArray, maxBitsPerEntryIndirect, bitsPerEntryDirect);
        subContainer.WriteTo(stream, state);
    }

    public static PalettedContainer ReadFrom(Stream stream, PacketState state, int length, byte maxBitsPerEntryIndirect, byte bitsPerEntryDirect)
    {
        var subContainer = PalettedContainerSubContainer.ReadFrom(stream, state, length, bitsPerEntryDirect);
        return new PalettedContainer(subContainer.GetData(length));
    }
}