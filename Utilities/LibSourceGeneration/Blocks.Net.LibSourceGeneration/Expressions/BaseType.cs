using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class BaseType(TypeReference reference, bool hasParams=false, params IExpression[] parameters) : IExpression
{
    public static implicit operator BaseType(TypeReference reference) => new(reference);
    public static implicit operator BaseType(string t) => new(t);

    public BaseType WithConstructorParameters(params IExpression[] newParameters) =>
        new(reference, true, newParameters);
    
    public string Generate()
    {
        var r = reference.Generate();
        if (hasParams)
        {
            r += $"({string.Join(", ", parameters.Select(x => x.Generate()))})";
        }
        return r;
    }
}