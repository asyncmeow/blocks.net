using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("boss_event", true, "Play")]
public partial class BossBar : IPacket
{
    [PacketField] public Uuid Uuid;
    [PacketEnum(typeof(VarInt))] public BossBarAction Action;

    [PacketSplitEnumDataField(nameof(Action))]
    public IBossBarAction ActionData;
}