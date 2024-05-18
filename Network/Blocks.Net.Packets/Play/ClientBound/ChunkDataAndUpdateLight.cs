using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x25,true,"Play")]
public partial class ChunkDataAndUpdateLight : IPacket
{
    [PacketField] public int ChunkX;
    [PacketField] public int ChunkZ;
    [PacketField] public NbtTag Heightmaps;
    [PacketField] public VarInt Size;
    [PacketArrayField("Size")] public byte[] Data;
    [PacketField] public VarInt BlockEntitiesCount;

    [PacketArrayField("BlockEntitiesCount")]
    public BlockEntity[] BlockEntities;

    [PacketField] public BitSet SkyLightMask;
    [PacketField] public BitSet BlockLightMask;
    [PacketField] public BitSet EmptySkyLightMask;
    [PacketField] public BitSet EmptyBlockLightMask;

    [PacketField] public VarInt SkyLightArrayCount;

    [PacketArrayField("SkyLightArrayCount")]
    public LightArray[] SkyLightArrays;

    [PacketField] public VarInt BlockLightArrayCount;

    [PacketArrayField("BlockLightArrayCount")]
    public LightArray[] BlockLightArrays;
}