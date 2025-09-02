using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_held_slot",true,"Play")]
public partial class SetHeldItem : IPacket
{
    [PacketField] public VarInt Slot;
}