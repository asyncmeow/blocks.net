using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet(0x04,true,"Configuration")]
public partial class Ping : IPacket
{
    [PacketField] public int Id;
}