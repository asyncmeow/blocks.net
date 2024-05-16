using Blocks.Net.LibSourceGeneration.Definitions;

namespace Blocks.Net.LibSourceGeneration.References;

public class TypeParameterReference(string name)
{
    public static implicit operator TypeParameterReference(string name) => new(name);
    
    public string Name => name;
    public Variance Variance => Variance.Invariant;

    public string Build() => Variance switch
    {
        Variance.Covariant => $"out {Name}",
        Variance.Contravariant => $"in {Name}",
        Variance.Invariant => $"{name}",
        _ => throw new ArgumentOutOfRangeException()
    };
}