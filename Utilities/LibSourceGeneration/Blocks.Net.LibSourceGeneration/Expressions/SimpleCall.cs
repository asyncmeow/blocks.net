using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class SimpleCall(string methodName, params IExpression[] arguments) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel) => builder
        .Append(methodName).Append('(').Join(", ",
            arguments.Select(x => x.Build(new StringBuilder(), indentation, indentationLevel).ToString())).Append(')');
}