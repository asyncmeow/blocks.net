using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("pick_item_from_block", false, "Play")]
public partial class PickItemFromBlock : IPacket
{
    [PacketField] public Position Location;
    [PacketField] public bool IncludeData;
}