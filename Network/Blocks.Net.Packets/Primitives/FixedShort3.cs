using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Primitives;

[SubPacket]
public partial struct FixedShort3
{
    [PacketField] public FixedShort4Point12 X;
    [PacketField] public FixedShort4Point12 Y;
    [PacketField] public FixedShort4Point12 Z;
}