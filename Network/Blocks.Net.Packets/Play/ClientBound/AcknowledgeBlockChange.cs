using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("block_changed_ack",true,"Play")]
public partial class AcknowledgeBlockChange : IPacket
{
    [PacketField] public VarInt SequenceId;
}