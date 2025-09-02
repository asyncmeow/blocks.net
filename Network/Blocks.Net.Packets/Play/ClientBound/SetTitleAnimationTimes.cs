using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_titles_animation",true,"Play")]
public partial class SetTitleAnimationTimes : IPacket
{
    [PacketField] public int FadeIn;
    [PacketField] public int Stay;
    [PacketField] public int FadeOut;
}