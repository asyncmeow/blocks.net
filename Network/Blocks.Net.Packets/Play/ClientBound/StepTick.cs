using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("ticking_step",true,"Play")]
public partial class StepTick : IPacket
{
    [PacketField] public VarInt TickSteps;
}