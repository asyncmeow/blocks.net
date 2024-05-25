using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x33,true,"Play")]
public partial class Ping : IPacket
{
    [PacketField] public int Id;
}