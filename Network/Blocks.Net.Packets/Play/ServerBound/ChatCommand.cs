using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("chat_command",false,"Play")]
public partial class ChatCommand : IPacket
{
    [PacketField] public string Command;
}