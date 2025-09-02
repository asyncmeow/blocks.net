using System.Security;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("chat",false,"Play")]
public partial class ChatMessage : IPacket
{
    [PacketField] public string Message;
    [PacketField] public long Timestamp;
    [PacketField] public long Salt;
    [PacketField] public ChatSignature? Signature;
    [PacketField] public VarInt MessageCount;
    [PacketField] public FixedBitSet20 Acknowledged;
    [PacketField] public byte Checksum;
}