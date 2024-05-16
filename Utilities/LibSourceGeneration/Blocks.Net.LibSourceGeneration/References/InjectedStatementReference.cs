using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.References;

public class InjectedStatementReference(string statement) : IBuildable
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        var lines = statement.Split('\n', '\r');
        foreach (var line in lines)
        {
            builder.AppendRepeating(indentation, indentationLevel).Append(line).Append('\n');
        }
        return builder;
    }
}