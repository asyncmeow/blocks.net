using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("resource_pack",false,"Play")]
public partial class ResourcePackResponse : IPacket
{
    public enum Results
    {
        Success,
        Declined,
        Failed,
        Accepted,
        Downloaded,
        InvalidUrl,
        FailedToReload,
        Discarded
    }
    [PacketField] public Uuid ResourcePack;
    [PacketEnum(typeof(VarInt))] public Results Result;
}