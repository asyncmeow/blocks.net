using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_passengers",true,"Play")]
public partial class SetPassengers : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public VarInt[] Passengers;
}