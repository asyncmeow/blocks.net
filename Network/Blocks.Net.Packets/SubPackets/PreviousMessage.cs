using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct PreviousMessage
{
    [PacketField] public VarInt MessageId;

    [PacketOptionalArrayField("MessageId == 0", "256")]
    public byte[] Signature;
}