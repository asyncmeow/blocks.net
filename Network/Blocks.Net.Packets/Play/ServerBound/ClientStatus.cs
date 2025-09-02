using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("client_command",false,"Play")]
public partial class ClientStatus : IPacket
{
    public enum Actions
    {
        PerformRespawn,
        RequestStatistics
    }
    
    [PacketEnum(typeof(VarInt))] public Actions Action;
}