using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Enums;

[FieldedEnum(typeof(VarInt))]
public enum NumberFormat
{
    Blank,
    Styled,
    Fixed
}

[EnumField(typeof(NumberFormat))]
public class NumberFormatStyled
{
    [PacketField] public NbtTag Styling;
}
[EnumField(typeof(NumberFormat))]
public class NumberFormatFixed
{
    [PacketField] public TextComponent Content;
}

