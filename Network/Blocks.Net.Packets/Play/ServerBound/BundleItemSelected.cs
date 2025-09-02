using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("bundle_item_selected",false,"Play")]
public partial class BundleItemSelected : IPacket
{
    [PacketField] public VarInt SlotOfBundle;
    [PacketField] public VarInt SlotInBundle;
}