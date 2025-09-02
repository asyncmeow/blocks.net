using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_border_warning_delay",true,"Play")]
public partial class SetBorderWarningDelay : IPacket
{
    [PacketField] public VarInt WarningTime;
}