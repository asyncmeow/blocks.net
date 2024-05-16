using System.Text;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class Default(TypeReference t) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel) =>
        builder.Append("default(").Append(t).Append(")");
}