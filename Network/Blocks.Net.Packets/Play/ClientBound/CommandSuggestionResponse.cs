using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x10,true,"Play")]
public partial class CommandSuggestionResponse : IPacket
{
    [PacketField] public VarInt TransactionId;
    [PacketField] public VarInt Start;
    [PacketField] public VarInt Length;
    [PacketField] public VarInt Count;
    [PacketArrayField("Count")] public SuggestionMatch[] Matches;
}