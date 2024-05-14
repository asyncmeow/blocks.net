using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ClientBound;

[PublicAPI]
[Packet(0x01,true,"Login")]
public partial class EncryptionRequest : IPacket
{
    [PacketField] public string ServerId;
    [PacketField] public VarInt PublicKeyLength;
    [PacketArrayField("PublicKeyLength")] public byte[] PublicKey;
    [PacketField] public VarInt VerifyTokenLength;
    [PacketArrayField("VerifyTokenLength")] public byte[] VerifyToken;
}