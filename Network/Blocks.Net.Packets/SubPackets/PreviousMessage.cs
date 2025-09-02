using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct PreviousMessage
{
    [PacketField] public VarInt MessageId;

    [PacketOptionalField("MessageId == 0")]
    public ChatSignature Signature;
}