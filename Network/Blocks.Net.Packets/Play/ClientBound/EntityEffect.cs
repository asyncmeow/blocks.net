using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("update_mob_effect",true,"Play")]
public partial class EntityEffect : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public VarInt EffectId;
    [PacketField] public VarInt Amplifier;
    [PacketField] public VarInt Duration;
    [PacketField] public byte Flags;
}