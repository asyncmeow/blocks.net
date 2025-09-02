using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("custom_click_action",false,"Play")]
public partial class CustomAction : IPacket
{
    [PacketField] public Identifier Id;
    [PacketField] public NbtTag Payload;
}