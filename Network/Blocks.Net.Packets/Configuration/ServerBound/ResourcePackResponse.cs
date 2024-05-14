using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ServerBound;

[PublicAPI]
[Packet(0x05,false,"Configuration")]
public partial class ResourcePackResponse : IPacket
{
    public enum ResultEnum
    {
        Success = 0,
        Declined = 1,
        DownloadFailed = 2,
        Accepted = 3,
        Downloaded = 4,
        InvalidUrl = 5,
        FailedToReload = 6,
        Discarded = 7
    }

    [PacketField] public Uuid Uuid;
    [PacketEnum(typeof(VarInt))] public ResultEnum Result;
}