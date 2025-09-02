using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("stop_sound",true,"Play")]
public partial class StopSound : IPacket
{
    public enum Sources
    {
        Master,
        Music,
        Record,
        Weather,
        Block,
        Hostile,
        Neutral,
        Player,
        Ambient,
        Voice
    }
    
    [PacketField] public byte Flags;

    [PacketOptionalField("Flags == 3 || Flags == 1")]
    private VarInt _source;

    [PacketOptionalField("Flags == 3 || Flags == 2")]
    public Identifier Sound;
    
    public Sources Source
    {
        get => (Sources)(int)_source;
        set => _source = (int)value;
    }
}