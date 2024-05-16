using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class InjectedExpression(string injection) : IExpression
{
    public string Generate() => injection;
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        var lines = injection.Split('\r', '\n');
        var indent = new StringBuilder().Append('\n').AppendRepeating(indentation, indentationLevel+1).ToString();
        return builder.Join(indent, lines);
    }
}