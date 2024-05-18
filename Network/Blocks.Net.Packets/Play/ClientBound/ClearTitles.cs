using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x0F,true,"Play")]
public partial class ClearTitles : IPacket
{
    [PacketField] public bool Reset;
}