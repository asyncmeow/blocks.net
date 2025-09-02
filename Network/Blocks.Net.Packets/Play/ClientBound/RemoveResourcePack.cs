using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("resource_pack_pop",true,"Play")]
public partial class RemoveResourcePack : IPacket
{
    [PacketField] public Uuid? Uuid;
}