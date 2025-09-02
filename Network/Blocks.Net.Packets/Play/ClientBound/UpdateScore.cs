using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_score",true,"Play")]
public partial class UpdateScore : IPacket
{
    [PacketField] public string EntityName;
    [PacketField] public string ObjectiveName;
    [PacketField] public VarInt Value;
    [PacketField] public TextComponent? DisplayName;
    [PacketField] public NumberFormatImpl? NumberFormat;
}