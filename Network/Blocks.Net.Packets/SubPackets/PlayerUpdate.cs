using Blocks.Net.Packets.Enums;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket(typeof(int))]
public partial struct PlayerUpdate
{
    [PacketField] public Guid PlayerUuid;
    [PacketArrayField("_0")] public PlayerActionImpl[] Actions;
}