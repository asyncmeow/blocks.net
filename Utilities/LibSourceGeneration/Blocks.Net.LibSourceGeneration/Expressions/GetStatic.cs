using System.Text;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class GetStatic(TypeReference t, string name) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel) =>
        builder.Append(t).Append('.').Append(name);
}