using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("set_carried_item",false,"Play")]
public partial class SetHeldItem : IPacket
{
    [PacketField] public short Slot;
}