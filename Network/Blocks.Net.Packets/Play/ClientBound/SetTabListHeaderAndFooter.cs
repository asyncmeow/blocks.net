using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("tab_list",true,"Play")]
public partial class SetTabListHeaderAndFooter : IPacket
{
    [PacketField] public TextComponent Header;
    [PacketField] public TextComponent Footer;
}