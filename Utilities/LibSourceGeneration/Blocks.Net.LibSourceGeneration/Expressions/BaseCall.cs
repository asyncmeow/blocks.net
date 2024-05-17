using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Expressions;

/// <summary>
/// Call a base constructor
/// </summary>
/// <param name="reference">The type of constructor to call, null if it's another constructor in the type</param>
public class BaseCall(TypeReference? reference = null) : IExpression
{
    private List<IExpression> _parameters = [];
    public TypeReference? Type => reference;
    public IEnumerable<IExpression> Parameters => _parameters;

    public BaseCall WithConstructorParameters(params IExpression[] newParameters)
    {
        _parameters.AddRange(newParameters);
        return this;
    }

    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel) =>
        builder.Append(reference?.Generate() ?? "this").Append('(').Join(", ",
                _parameters.Select(x => x.Build(new StringBuilder(), indentation, indentationLevel + 1).ToString()))
            .Append(')');
}