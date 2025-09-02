using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Primitives;

[SubPacket]
public partial struct Short3
{
    [PacketField] public short X;
    [PacketField] public short Y;
    [PacketField] public short Z;
}