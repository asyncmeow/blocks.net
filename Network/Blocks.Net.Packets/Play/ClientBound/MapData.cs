using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.MapData;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x2A, true, "Play")]
public partial class MapData : IPacket
{
    [PacketField] public VarInt MapId;
    [PacketField] public byte Scale;
    [PacketField] public bool Locked;
    [PacketField] public bool HasIcons;
    [PacketOptionalField("HasIcons")] public VarInt IconCount;

    [PacketOptionalArrayField("HasIcons", "IconCount")]
    public MapIcon[] Icons;

    [PacketField] public MapColorPatch ColorPatch;

}