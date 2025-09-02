using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_border_center", true, "Play")]
public partial class SetBorderCenter : IPacket
{
    [PacketField] public double X;
    [PacketField] public double Z;
}