using System.Text;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public abstract class BinaryExpression(IExpression lhs, IExpression rhs) : IExpression
{
    public abstract string Operator { get; }

    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.Append('(');
        lhs.Build(builder, indentation, indentationLevel).Append(") ").Append(Operator).Append(" (");
        return rhs.Build(builder, indentation, indentationLevel).Append(')');
    }
}