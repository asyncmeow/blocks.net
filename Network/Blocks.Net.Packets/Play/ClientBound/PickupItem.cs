using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("take_item_entity",true,"Play")]
public partial class PickupItem : IPacket
{
    [PacketField] public VarInt CollectedEntityId;
    [PacketField] public VarInt CollectorEntityId;
    [PacketField] public VarInt PickupItemCount;
}