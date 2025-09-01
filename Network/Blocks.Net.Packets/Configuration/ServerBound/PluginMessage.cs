using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ServerBound;

[PublicAPI]
[Packet("custom_payload",false,"Configuration")]
public partial class PluginMessage : IPacket
{
    [PacketField] public Identifier Channel;
    [PacketField] public LengthInferredByteArray Data;
}