using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_default_spawn_position",true,"Play")]
public partial class SetDefaultSpawnPosition : IPacket
{
    [PacketField] public Position Location;
    [PacketField] public float Angle;
}