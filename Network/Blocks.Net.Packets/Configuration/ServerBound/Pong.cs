using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ServerBound;

[PublicAPI]
[Packet(0x04,false,"Configuration")]
public partial class Pong : IPacket
{
    [PacketField] public int Id;
}