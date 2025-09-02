using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.Packets.SubPackets.Advancements;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("update_advancements",true,"Play")]
public partial class UpdateAdvancements : IPacket
{
    [PacketField] public bool Reset;
    [PacketField] public AdvancementMapping[] Mappings;
    [PacketField] public Identifier[] Identifiers;
    [PacketField] public AdvancementProgress[] Progress;
    [PacketField] public bool ShowAdvancements;
}