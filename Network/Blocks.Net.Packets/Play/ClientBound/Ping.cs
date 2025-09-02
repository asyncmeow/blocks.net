using Blocks.Net.Packets.Configuration.ClientBound;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("ping",true,"Play")]
public partial class Ping : IPacket
{
    [PacketField] public int Id;
}