using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("select_advancements_tab",true,"Play")]
public partial class SelectAdvancementsTab : IPacket
{
    [PacketField] public Identifier? Tab;
}