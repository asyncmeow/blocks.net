using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("player_command",false,"Play")]
public partial class PlayerCommand : IPacket
{
    public enum Actions
    {
        LeaveBed,
        StartSprinting,
        StopSprinting,
        StartJumpWithHorse,
        StopJumpWithHorse,
        OpenVehicleInventory,
        StartFlyingWithElytra
    }
    [PacketField] public VarInt EntityId;
    [PacketEnum(typeof(VarInt))] public Actions Action;
    [PacketField] public VarInt JumpBoost;
}