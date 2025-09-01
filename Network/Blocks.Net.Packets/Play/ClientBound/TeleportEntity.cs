using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Double3 = Blocks.Net.Packets.Primitives.Double3;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("entity_position_sync", true, "Play")]
public partial class TeleportEntity : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public Double3 Position;
    [PacketField] public Double3 Velocity;
    [PacketField] public float Yaw;
    [PacketField] public float Pitch;
    [PacketField] public bool OnGround;
}