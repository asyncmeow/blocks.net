using Blocks.Net.Packets.Enums;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x09,true,"Play")]
public partial class BossBar : IPacket
{
    [PacketField] public Guid BarUuid;
    [PacketField] public BossBarActionImpl Action;
}