using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x1F,true,"Play")]
public partial class UnloadChunk : IPacket
{
    [PacketField] public int Z;
    [PacketField] public int X;
}