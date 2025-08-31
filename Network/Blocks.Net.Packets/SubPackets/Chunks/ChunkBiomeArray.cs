using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[RequiresStateField(typeof(int),"DimensionSize")]
public readonly struct ChunkBiomeArray(ChunkBiomeSection[] chunkData)
{
    public readonly ChunkBiomeSection[] ChunkData = chunkData;
    private static readonly ThreadLocal<byte[]> BackingBuffer = new(() => new byte[48 * 4096 * 2]);

    public void WriteTo(Stream stream, PacketState state)
    {
        using var memoryStream = new MemoryStream(BackingBuffer.Value!);
        foreach (var data in ChunkData)
        {
            data.WriteTo(memoryStream, state);
        }

        if (memoryStream.Position > BackingBuffer.Value!.Length)
        {
            BackingBuffer.Value = new byte[memoryStream.Position * 2];
        }
        
        new VarInt((int)memoryStream.Position).WriteTo(stream, state);
        var numBytes = memoryStream.Position;
        var buffer = memoryStream.GetBuffer();
        stream.Write(buffer, 0, (int)numBytes);
    }

    public static ChunkBiomeArray ReadFrom(Stream stream, PacketState state)
    {
        ChunkBiomeSection[] chunkData = new ChunkBiomeSection[state.DimensionSize];
        var dataCount = VarInt.ReadFrom(stream,state);
        if (dataCount > BackingBuffer.Value!.Length)
        {
            BackingBuffer.Value = new byte[dataCount * 2];
        }
        stream.ReadExactly(BackingBuffer.Value!, 0, dataCount);
        using var stream2 = new MemoryStream(BackingBuffer.Value!);
        for (var i = 0; i < state.DimensionSize; i++)
        {
            chunkData[i] = ChunkBiomeSection.ReadFrom(stream2, state);
        }
        return new ChunkBiomeArray(chunkData);
    }
}