using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ServerBound;


[PublicAPI]
[Packet("key",false,"Login")]
public partial class EncryptionResponse : IPacket
{
    [PacketField] public byte[] SharedSecret;
    [PacketField] public byte[] VerifyToken;
}