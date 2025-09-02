using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Primitives;

[SubPacket]
public partial struct Byte3
{
    [PacketField] public byte R;
    [PacketField] public byte G;
    [PacketField] public byte B;
}