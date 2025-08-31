using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.MapData;

[SubPacket]
public partial struct MapColorPatch
{
    [PacketField] public byte Columns;
    [PacketOptionalField("Columns > 0")] public MapColorPatchInformation ColorPatchInformation;
}