using System.Text;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class PostIncrement(IExpression incrementee) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel) =>
        incrementee.Build(builder, indentation, indentationLevel).Append("++");
}