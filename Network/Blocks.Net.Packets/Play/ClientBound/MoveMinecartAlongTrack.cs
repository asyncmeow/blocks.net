using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("move_minecart_along_track",true,"Play")]
public partial class MoveMinecartAlongTrack : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public MinecartStep[] Steps;
}