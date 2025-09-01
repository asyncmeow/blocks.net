using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet("transfer", true, "Configuration")]
public partial class Transfer : IPacket
{
    [PacketField] public string Host;
    [PacketField] public VarInt Port;
}