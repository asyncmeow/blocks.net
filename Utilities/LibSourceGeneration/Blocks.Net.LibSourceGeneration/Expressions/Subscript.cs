using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class Subscript(IExpression toSubscript, params IExpression[] indices) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.Append('(');
        toSubscript.Build(builder, indentation, indentationLevel);
        return builder.Append(")[").Join(", ",
            indices.Select(x => x.Build(new StringBuilder(), indentation, indentationLevel).ToString())).Append(']');
    }
}