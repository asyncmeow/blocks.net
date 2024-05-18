using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x09,true,"Play")]
public partial class BlockUpdate : IPacket
{
    [PacketField] public Position Location;
    [PacketField] public VarInt BlockId;
}