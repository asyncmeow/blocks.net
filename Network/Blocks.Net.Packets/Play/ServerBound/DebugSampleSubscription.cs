using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("debug_sample_subscription",false,"Play")]
public partial class DebugSampleSubscription : IPacket
{
    [PacketField] public VarInt SampleType = 0;
}