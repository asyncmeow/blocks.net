using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("reset_score",true,"Play")]
public partial class ResetScore : IPacket
{
    [PacketField] public string EntityName;
    [PacketField] public string? ObjectiveName;
}