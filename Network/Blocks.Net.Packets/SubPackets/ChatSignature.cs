using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct ChatSignature
{
    public ChatSignature()
    {
    }
    
    [PacketArrayField("256")] public byte[] Data = new byte[256];

    public byte this[int index]
    {
        get => Data[index];
        set => Data[index] = value;
    }
}