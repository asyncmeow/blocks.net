using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_display_objective", true, "Play")]
public partial class DisplayObjective : IPacket
{
    [PacketField] public VarInt Position;
    [PacketField] public string Name;
}