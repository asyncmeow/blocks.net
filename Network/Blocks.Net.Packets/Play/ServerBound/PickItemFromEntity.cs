using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("pick_item_from_entity", false, "Play")]
public partial class PickItemFromEntity : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public bool IncludeData;
}