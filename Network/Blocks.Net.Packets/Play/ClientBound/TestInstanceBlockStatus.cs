using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("test_instance_block_status",true,"Play")]
public partial class TestInstanceBlockStatus : IPacket
{
    [PacketField] public TextComponent Status;
    [PacketField] public Double3? Size;
}