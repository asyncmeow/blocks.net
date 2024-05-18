using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;


[Packet(0x28,true,"Play")]
public partial class UpdateLight : IPacket
{
    [PacketField] public VarInt ChunkX;
    [PacketField] public VarInt ChunkZ;
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