using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x13,true,"Play")]
public partial class SetContainerProperty : IPacket
{
    [PacketField] public VarInt WindowId;
    [PacketField] public short Property;
    [PacketField] public short Value;
}