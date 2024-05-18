using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x14,true,"Play")]
public partial class SetContainerProperty : IPacket
{
    [PacketField] public byte WindowId;
    [PacketField] public short Property;
    [PacketField] public short Value;
}