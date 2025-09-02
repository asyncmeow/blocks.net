using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_time",true,"Play")]
public partial class UpdateTime : IPacket
{
    [PacketField] public long WorldAge;
    [PacketField] public long TimeOfDay;
    [PacketField] public bool TimeOfDayIncreasing;
}