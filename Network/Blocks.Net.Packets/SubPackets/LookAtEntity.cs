using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct LookAtEntity
{
    [PacketField]
    public VarInt EntityId;
    [PacketEnum(typeof(VarInt))]
    public AimingPositions EntityAimingPosition;
}