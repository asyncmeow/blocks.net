using Blocks.Net.Packets.SubPackets.Configuration;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Play.ClientBound;

[PublicAPI]
[Packet("server_links",true, "Play")]
public partial class ServerLinks : IPacket
{
    [PacketField] public ServerLink[] Links;
}