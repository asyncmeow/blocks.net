using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.MapData;

[SubPacket]
public partial struct MapColorPatch
{
    [PacketField] public byte Columns;
    [PacketOptionalField("Columns > 0")] public byte Rows;
    [PacketOptionalField("Columns > 0")] public byte X;
    [PacketOptionalField("Columns > 0")] public byte Z;
    [PacketOptionalField("Columns > 0")] public VarInt Length;

    [PacketOptionalArrayField("Columns > 0", "Length")]
    public byte[] Data;
}