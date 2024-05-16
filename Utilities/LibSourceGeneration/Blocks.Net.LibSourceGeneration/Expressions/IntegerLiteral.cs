using System.Numerics;
using System.Text;
using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class IntegerLiteral(BigInteger literal, IntegerType type=IntegerType.SignedInt, IntegerBase @base=IntegerBase.Decimal) : IExpression
{
    public string Generate()
    {
        var start = @base switch
        {
            IntegerBase.Decimal => $"{literal}",
            IntegerBase.Binary => $"0b{literal:B}",
            IntegerBase.Hexadecimal => $"0x{literal:X}",
            _ => throw new ArgumentOutOfRangeException(nameof(@base), @base, null)
        };
        return type switch
        {
            IntegerType.UnsignedInt => $"{start}U",
            IntegerType.SignedInt => start,
            IntegerType.UnsignedLong => $"{start}UL",
            IntegerType.SignedLong => $"{start}L",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel) =>
        builder.Append(Generate());
}