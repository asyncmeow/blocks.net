using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("custom_chat_completions",true,"Play")]
public partial class ChatSuggestions : IPacket
{
    public enum Actions
    {
        Add,
        Remove,
        Set
    }
    [PacketEnum(typeof(VarInt))] public Actions Action;
    [PacketField] public string[] Entries;
}