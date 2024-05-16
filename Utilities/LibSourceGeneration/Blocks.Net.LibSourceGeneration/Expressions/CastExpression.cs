using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class CastExpression(TypeReference to, IExpression expr) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.Append('(').Append(to).Append(')').Append('(');
        expr.Build(builder, indentation, indentationLevel);
        return builder.Append(')');
    }
}