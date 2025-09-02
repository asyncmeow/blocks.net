using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("paddle_boat",false,"Play")]
public partial class PaddleBoat : IPacket
{
    [PacketField] public bool LeftPaddleTurning;
    [PacketField] public bool RightPaddleTurning;
}