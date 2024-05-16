using System.Globalization;
using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class BoundCall(IExpression binder, params IExpression[] arguments) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        if (binder is Variable or GetField or Subscript or GetStatic)
        {
            return binder.Build(builder, indentation, indentationLevel).Append('(').Join(", ",
                    arguments.Select(x => x.Build(new StringBuilder(), indentation, indentationLevel).ToString()))
                .Append(')');
        }

        builder.Append('(');
        return binder.Build(builder, indentation, indentationLevel).Append(')').Append('.')
            .Append('(').Join(", ",
                arguments.Select(x => x.Build(new StringBuilder(), indentation, indentationLevel).ToString()))
            .Append(')');
    }
}