using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct ArgumentSignature
{
    [PacketField] public string ArgumentName;
    [PacketField] public ChatSignature Signature;
}