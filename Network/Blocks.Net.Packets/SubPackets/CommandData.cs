using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;


[SubPacket]
public partial struct CommandData
{
    public enum NodeType
    {
        Root = 0x00,
        Literal = 0x01,
        Argument = 0x02
    }
    
    [PacketField] public byte Flags;

    public NodeType Type
    {
        get => (NodeType)(Flags & 0b11);
        set => Flags = (byte)((Flags & 0b11111100) | (int)value);
    }

    public bool Executable
    {
        get => (Flags & 0x04) != 0;
        set => Flags = (byte)(value ? Flags | 0x04 : Flags & ~0x04);
    }
    
    public bool HasRedirect
    {
        get => (Flags & 0x08) != 0;
        set => Flags = (byte)(value ? Flags | 0x08 : Flags & ~0x08);
    }
    
    public bool HasSuggestionsType
    {
        get => (Flags & 0x10) != 0;
        set => Flags = (byte)(value ? Flags | 0x10 : Flags & ~0x10);
    }

    [PacketField] public VarInt ChildCount;
    [PacketArrayField("ChildCount")] public VarInt[] Children;

    [PacketOptionalField("(Flags & 0x08) != 0")]
    public VarInt RedirectNode;

    [PacketOptionalField("(Flags & 0x03) != 0")]
    public string Name;

    [PacketOptionalField("(Flags & 0x03) == 2")]
    public CommandParserImpl Parser;

    [PacketOptionalField("(Flags & 0x10) != 0")]
    public string SuggestionsType;
}