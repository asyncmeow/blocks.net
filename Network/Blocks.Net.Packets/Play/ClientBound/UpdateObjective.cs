using Blocks.Net.Packets.Enums;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_objective",true,"Play")]
public partial class UpdateObjective : IPacket
{
    public enum Modes
    {
        CreateScoreboard,
        RemoveScoreboard,
        UpdateDisplayText
    }

    public enum Types
    {
        Integer,
        Hearts
    }
    
    [PacketField] public string ObjectiveName;
    [PacketEnum(typeof(byte))] public Modes Mode;

    [PacketOptionalField("Mode != Modes.RemoveScoreboard")]
    public TextComponent ObjectiveValue;

    [PacketOptionalField("Mode != Modes.RemoveScoreboard")]
    public bool HasNumberFormat;
    [PacketOptionalField("Mode != Modes.RemoveScoreboard && HasNumberFormat")]
    public NumberFormatImpl Format;
}