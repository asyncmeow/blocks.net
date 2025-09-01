using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Chunks;

[SubPacket]
public partial struct LightArray
{
    [PacketField] public byte[] Array;
}