using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("remove_entities",true,"Play")]
public partial class RemoveEntities : IPacket
{
    [PacketField] public VarInt[] EntityIds;
}