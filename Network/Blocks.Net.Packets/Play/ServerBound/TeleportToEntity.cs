using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("teleport_to_entity",false,"Play")]
public partial class TeleportToEntity : IPacket
{
    [PacketField] public Uuid TargetPlayer;
}