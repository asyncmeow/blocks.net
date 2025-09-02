using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("container_button_click",false,"Play")]
public partial class ClickContainerButton : IPacket
{
    [PacketField] public VarInt WindowId;
    [PacketField] public VarInt ButtonId;
}