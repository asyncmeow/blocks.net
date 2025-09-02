using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("player_action",false,"Play")]
public partial class PlayerAction : IPacket
{
    public enum Actions
    {
        StartedDigging,
        CancelledDigging,
        FinishedDigging,
        DropItemStack,
        DropItem,
        ShootArrowOrFinishEating,
        SwapItemInHand
    }

    public enum Faces
    {
        NegativeY,
        PositiveY,
        NegativeZ,
        PositiveZ,
        NegativeX,
        PositiveX,
    }

    [PacketEnum(typeof(VarInt))] public Actions Action;
    [PacketField] public Position Location;
    [PacketEnum(typeof(byte))] public Faces Face;
    [PacketField] public VarInt Sequence;
}