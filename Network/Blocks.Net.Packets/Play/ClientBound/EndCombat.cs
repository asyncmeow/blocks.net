using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x38,true,"Play")]
public partial class EndCombat : IPacket
{
    [PacketField] public VarInt Duration;
}