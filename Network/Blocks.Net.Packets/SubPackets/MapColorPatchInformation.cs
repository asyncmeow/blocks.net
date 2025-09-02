using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial class MapColorPatchInformation
{
    
    [PacketField] public byte Rows;
    [PacketField] public byte X;
    [PacketField] public byte Z;
    [PacketField] public byte[]? Data;
}