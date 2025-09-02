using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct PublicKey
{
    [PacketField] public long ExpiresAt;
    [PacketField] public byte[] Key;
    [PacketField] public byte[] KeySignature;
}