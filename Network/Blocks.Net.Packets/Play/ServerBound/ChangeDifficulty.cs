using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("change_difficulty",false,"Play")]
public partial class ChangeDifficulty : IPacket
{
    [PacketField] public byte NewDifficulty;
}