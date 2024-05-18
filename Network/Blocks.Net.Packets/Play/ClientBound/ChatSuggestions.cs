using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x17,true,"Play")]
public partial class ChatSuggestions : IPacket
{
    public enum SuggestionAction
    {
        Add = 0,
        Remove = 1,
        Set = 2
    }

    [PacketEnum(typeof(VarInt))] public SuggestionAction Action;
    [PacketField] public VarInt Count;
    [PacketArrayField("Count")] public string[] Entries;
}