using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("debug_sample",true,"Play")]
public partial class DebugSample : IPacket
{
    [PacketField] public long[] Sample;
    [PacketField] public VarInt SampleType = 0;
}