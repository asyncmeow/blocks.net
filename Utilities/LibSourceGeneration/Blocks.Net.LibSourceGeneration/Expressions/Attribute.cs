using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class Attribute(TypeReference type) : IExpression
{
    private List<IExpression> _parameters = [];

    public IExpression WithParameters(params IExpression[] parameters)
    {
        _parameters.AddRange(parameters);
        return this;
    }
    
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.Append(type);
        if (_parameters.Count > 0)
        {
            builder.Append('(').Join(", ",
                    _parameters.Select(x => x.Build(new StringBuilder(), indentation, indentationLevel + 1).ToString()))
                .Append(')');
        }
        return builder;
    }
}