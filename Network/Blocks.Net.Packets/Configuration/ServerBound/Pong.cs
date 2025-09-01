using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ServerBound;

[PublicAPI]
[Packet("pong",false,"Configuration")]
public partial class Pong : IPacket
{
    [PacketField] public int Id;
}