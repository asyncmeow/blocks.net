using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_chunk_cache_radius",true,"Play")]
public partial class SetRenderDistance : IPacket
{
    [PacketField] public VarInt ViewDistance;
}