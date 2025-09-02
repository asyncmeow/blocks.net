using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("pong",false,"Play")]
public partial class Pong : IPacket
{
    [PacketField] public int Id;
}