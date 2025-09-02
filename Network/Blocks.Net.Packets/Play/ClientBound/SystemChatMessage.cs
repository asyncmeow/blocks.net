using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("system_chat",true,"Play")]
public partial class SystemChatMessage : IPacket
{
    [PacketField] public TextComponent Content;
    [PacketField] public bool Overlay;
}