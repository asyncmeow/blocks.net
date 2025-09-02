using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("change_game_mode",false,"Play")]
public partial class ChangeGameMode : IPacket
{
    [PacketField] public VarInt GameMode;
}