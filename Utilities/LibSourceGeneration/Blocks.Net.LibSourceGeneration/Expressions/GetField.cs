using System.Text;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class GetField(IExpression lhs, string fieldName) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        if (lhs is Variable or GetField or Subscript or GetStatic)
        {
            return lhs.Build(builder, indentation, indentationLevel).Append('.').Append(fieldName);
        }

        builder.Append('(');
        return lhs.Build(builder, indentation, indentationLevel).Append(").").Append(fieldName);
    }
}