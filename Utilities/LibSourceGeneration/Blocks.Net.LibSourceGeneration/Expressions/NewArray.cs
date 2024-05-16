using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class NewArray(TypeReference arrayType, params IExpression[] sizes) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel) => builder.Append("new ").Append(arrayType).Append('[').Join(", ",
            sizes.Select(x => x.Build(new StringBuilder(), indentation, indentationLevel).ToString())).Append(']');
}