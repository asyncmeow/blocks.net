using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.Packets.SubPackets.Configuration;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Double3 = Blocks.Net.Packets.Primitives.Double3;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("damage_event",true,"Play")]
public partial class DamageEvent : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public RegistryEntry SourceTypeId;
    [PacketField] public VarInt SourceCauseId;
    [PacketField] public VarInt SourceDirectId;
    [PacketField] public Double3? SourcePosition;
}