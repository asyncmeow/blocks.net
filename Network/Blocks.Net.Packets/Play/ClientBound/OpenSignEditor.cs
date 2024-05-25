using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x32,true,"Play")]
public partial class OpenSignEditor : IPacket
{
    [PacketField] public Position Location;
    [PacketField] public bool IsFrontText;
}