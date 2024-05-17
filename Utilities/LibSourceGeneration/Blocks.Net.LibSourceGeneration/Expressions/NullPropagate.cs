using System.Text;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class NullPropagate(IExpression expression) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        if (expression is Variable or GetField or Subscript or GetStatic or NullPropagate)
        {
            return expression.Build(builder, indentation, indentationLevel).Append('?');
        }

        builder.Append('(');
        return expression.Build(builder, indentation, indentationLevel).Append(")?");
    }
}