using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.References;

public class Attribute(TypeReference type) : IExpression
{
    private List<IExpression> _parameters = [];

    public IExpression WithParameters(params IExpression[] parameters)
    {
        _parameters.AddRange(parameters);
        return this;
    }

    public string Generate() => _parameters.Count == 0
        ? type
        : $"{type}({string.Join(", ", _parameters.Select(x => x.Generate()))}";
}