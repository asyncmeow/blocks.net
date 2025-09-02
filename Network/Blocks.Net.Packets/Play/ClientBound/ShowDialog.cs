using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("show_dialog",true,"Play")]
public partial class ShowDialog : IPacket
{
    [PacketField] public IdOrNbt Dialog;
}