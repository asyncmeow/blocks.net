using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_border_warning_distance",true,"Play")]
public partial class SetBorderWarningDistance : IPacket
{
    [PacketField] public VarInt WarningBlocks;
}