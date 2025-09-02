using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("accept_teleportation",false,"Play")]
public partial class ConfirmTeleportation : IPacket
{
    [PacketField] public VarInt TeleportId;
}