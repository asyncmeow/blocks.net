using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_title_text",true,"Play")]
public partial class SetTitleText : IPacket
{
    [PacketField] public TextComponent TitleText;
    
}