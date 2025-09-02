using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_border_lerp_size",true,"Play")]
public partial class SetBorderLerpSize : IPacket
{
    [PacketField] public double OldDiameter;
    [PacketField] public double NewDiameter;
    [PacketField] public VarLong Speed;
}