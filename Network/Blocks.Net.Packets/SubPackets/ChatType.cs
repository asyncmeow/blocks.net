using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
[GenerateIdOrXFor]
public partial struct ChatType
{
    [PacketField] public ChatDecoration Chat;
    [PacketField] public ChatDecoration Narration;
}