using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x04,true,"Play")]
public partial class AcknowledgeBlockChange : IPacket
{
    [PacketField] public VarInt SequenceId;
}