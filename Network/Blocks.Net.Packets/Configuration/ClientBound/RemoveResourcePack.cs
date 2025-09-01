using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet("resource_pack_pop", true,"Configuration")]
public partial class RemoveResourcePack : IPacket
{
    [PacketField] public Uuid? Uuid;
}