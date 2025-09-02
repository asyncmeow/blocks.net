using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("server_data",true,"Play")]
public partial class ServerData : IPacket
{
    [PacketField] public TextComponent Motd;
    [PacketField] public byte[]? Icon;
}