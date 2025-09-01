using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("command_suggestions", true, "Play")]
public partial class CommandSuggestionResponse : IPacket
{
    [PacketField] public VarInt Id;
    [PacketField] public VarInt Start;
    [PacketField] public VarInt Length;
    [PacketField] public SuggestionMatch[] Matches;
}