using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ServerBound;

[PublicAPI]
[Packet(0x02,false,"Login")]
public partial class LoginPluginResponse : IPacket
{
    [PacketField] public VarInt MessageId;
    [PacketField] public bool Successful;
    [PacketField] public LengthInferredByteArray Data;
}