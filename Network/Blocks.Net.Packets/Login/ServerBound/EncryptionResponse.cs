using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ServerBound;


[PublicAPI]
[Packet(0x01,false,"Login")]
public partial class EncryptionResponse : IPacket
{
    [PacketField] public VarInt SharedSecretLength;
    [PacketArrayField("SharedSecretLength")] public byte[] SharedSecret;
    [PacketField] public VarInt VerifyTokenLength;
    [PacketArrayField("VerifyTokenLength")] public byte[] VerifyToken;
}