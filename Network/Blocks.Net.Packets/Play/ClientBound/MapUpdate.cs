using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.MapData;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("map_item_data",true,"Play")]
public partial class MapUpdate : IPacket
{
    [PacketField] public VarInt MapId;
    [PacketField] public byte Scale;
    [PacketField] public bool Locked;
    [PacketField] public MapIcon[] Icons;
    [PacketField] public MapColorPatch ColorPatch;
}