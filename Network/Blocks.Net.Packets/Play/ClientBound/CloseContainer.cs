using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x10,true,"Play")]
public partial class CloseContainer : IPacket
{
    [PacketField] public VarInt WindowId;
}