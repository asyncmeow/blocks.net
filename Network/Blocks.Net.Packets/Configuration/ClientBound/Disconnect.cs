using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[Packet(0x01,true,"Configuration")]
public partial class Disconnect : IPacket
{
    
}