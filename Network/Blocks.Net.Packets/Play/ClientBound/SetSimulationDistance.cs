using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_simulation_distance",true,"Play")]
public partial class SetSimulationDistance : IPacket
{
    [PacketField] public VarInt Distance;
}