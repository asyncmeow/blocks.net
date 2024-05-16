using System.Text;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class SubscriptInitializer(IExpression subscript, IExpression value) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.Append('[');
        subscript.Build(builder, indentation, indentationLevel);
        builder.Append("] = ");
        return value.Build(builder, indentation, indentationLevel);
    }
}