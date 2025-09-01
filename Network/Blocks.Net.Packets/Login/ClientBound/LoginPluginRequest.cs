using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ClientBound;

[PublicAPI]
[Packet("custom_query",true,"Login")]
public partial class LoginPluginRequest : IPacket
{
    [PacketField] public VarInt MessageId;
    [PacketField] public Identifier Channel;
    [PacketField] public LengthInferredByteArray Data;
}