using System.Text;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class FieldInitializer(string field, IExpression initializer) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.Append(field).Append(" = ");
        return initializer.Build(builder, indentation, indentationLevel);
    }
}