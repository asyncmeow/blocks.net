using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("chat_ack",false,"Play")]
public partial class AcknowledgeMessage : IPacket
{
    [PacketField] public VarInt MessageCount;
}