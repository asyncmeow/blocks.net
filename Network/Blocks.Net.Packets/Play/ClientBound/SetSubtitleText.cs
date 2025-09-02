using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_subtitle_text",true,"Play")]
public partial class SetSubtitleText : IPacket
{
    [PacketField] public TextComponent SubtitleText;
}