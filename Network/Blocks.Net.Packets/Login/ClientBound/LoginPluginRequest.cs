using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ClientBound;

[PublicAPI]
[Packet(0x04,true,"Login")]
public partial class LoginPluginRequest : IPacket
{
    [PacketField] public VarInt MessageId;
    [PacketField] public string Channel;
    [PacketField] public LengthInferredByteArray Data;
}