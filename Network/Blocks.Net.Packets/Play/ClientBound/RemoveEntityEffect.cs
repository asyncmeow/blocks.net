using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("remove_mob_effect",true,"Play")]
public partial class RemoveEntityEffect : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public VarInt EffectId;
    
}