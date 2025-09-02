using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_action_bar_text",true,"Play")]
public partial class SetActionBarText : IPacket
{
    [PacketField] public TextComponent ActionBarText;
}