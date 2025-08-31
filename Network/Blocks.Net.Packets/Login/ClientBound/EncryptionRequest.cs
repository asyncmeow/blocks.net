using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;
using Byte = Blocks.Net.Packets.Primitives.Byte;

namespace Blocks.Net.Packets.Login.ClientBound;

[PublicAPI]
[Packet(0x01,true,"Login")]
public partial class EncryptionRequest : IPacket
{
    [PacketField] public string ServerId;
    [PacketField] public byte[] PublicKey;
    [PacketField] public byte[] VerifyToken;
}