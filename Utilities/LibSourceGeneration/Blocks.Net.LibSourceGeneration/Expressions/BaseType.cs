using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class BaseType(TypeReference reference, bool hasParams = false, params IExpression[] parameters) : IExpression
{
    public static implicit operator BaseType(TypeReference reference) => new(reference);
    public static implicit operator BaseType(string t) => new(t);

    public BaseType WithConstructorParameters(params IExpression[] newParameters) =>
        new(reference, true, newParameters);

    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.Append(reference.Generate());
        if (hasParams)
            builder.Append('(').Join(", ",
                    parameters.Select(x => x.Build(new StringBuilder(), indentation, indentationLevel + 1).ToString()))
                .Append(')');
        return builder;
    }
}