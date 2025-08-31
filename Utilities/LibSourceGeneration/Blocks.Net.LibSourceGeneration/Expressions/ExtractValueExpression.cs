using System.Linq.Expressions;
using System.Text;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class ExtractValueExpression(IExpression value, string newVariableName) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.Append('(');
        builder = value.Build(builder, indentation, indentationLevel);
        builder.Append($") is {{}} {newVariableName}");
        return builder;
    }
}