using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("command_suggestion",false,"Play")]
public partial class CommandSuggestionsRequest : IPacket
{
    [PacketField] public VarInt TransactionId;
    [PacketField] public string Text;
}