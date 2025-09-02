using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Primitives;

[SubPacket]
public partial struct Byte3
{
    [PacketField] public sbyte X;

    public byte R
    {
        get => (byte)X;
        set => X = (sbyte)value;
    }
    
    [PacketField] public sbyte Y;
    
    public byte G
    {
        get => (byte)Y;
        set => Y = (sbyte)value;
    }
    
    [PacketField] public sbyte Z;
    
    
    public byte B
    {
        get => (byte)Z;
        set => Z = (sbyte)value;
    }
}