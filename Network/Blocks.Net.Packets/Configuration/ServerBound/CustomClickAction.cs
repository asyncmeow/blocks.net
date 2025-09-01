using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ServerBound;

[PublicAPI]
[Packet("custom_click_action",false,"Configuration")]
public partial class CustomClickAction : IPacket
{
    [PacketField] public Identifier Id;
    [PacketField] public NbtTag Payload;
}