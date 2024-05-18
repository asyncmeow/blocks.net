using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct LightArray
{
    [PacketField] public VarInt Length;
    [PacketArrayField("Length")] public byte[] Array;
}