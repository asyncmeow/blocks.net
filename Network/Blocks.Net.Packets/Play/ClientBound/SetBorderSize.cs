using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_border_size",true,"Play")]
public partial class SetBorderSize : IPacket
{
    [PacketField] public double Diameter;
}