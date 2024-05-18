using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x12,true,"Play")]
public partial class CloseContainer : IPacket
{
    [PacketField] public byte WindowId;
}