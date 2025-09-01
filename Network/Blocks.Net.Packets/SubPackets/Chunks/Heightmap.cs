using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Chunks;

[SubPacket]
[RequiresStateField(typeof(byte),"CurrentDimensionHeightmapBitsPerEntry")]
public partial struct Heightmap
{
    public enum Types
    {
        WorldSurface = 1,
        MotionBlocking = 4,
        MotionBlockingNoLeaves = 5
    }
    
    [PacketEnum(typeof(VarInt))] 
    public Types Type;

    [PacketField("256","state.CurrentDimensionHeightmapBitsPerEntry")] public LongPackedDataArray Data;
}