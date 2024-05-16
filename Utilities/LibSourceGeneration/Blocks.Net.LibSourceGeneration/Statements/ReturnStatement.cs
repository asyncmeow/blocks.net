using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Statements;

public class ReturnStatement(IExpression? expr) : IStatement
{
    public IExpression? Expression => expr;
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.AppendRepeating(indentation, indentationLevel).Append("return");
        if (expr != null)
        {
            builder.Append(' ');
            expr.Build(builder, indentation, indentationLevel);
        }
        return builder.Append(";\n");
    }
}