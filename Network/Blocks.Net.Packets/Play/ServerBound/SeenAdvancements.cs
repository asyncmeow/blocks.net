using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("seen_advancements", false, "Play")]
public partial class SeenAdvancements : IPacket
{
    public enum Actions
    {
        OpenedTab,
        ClosedScreen
    }

    [PacketEnum(typeof(VarInt))] public Actions Action;

    [PacketOptionalField("Action == Actions.OpenedTab")]
    public Identifier TabId;
}