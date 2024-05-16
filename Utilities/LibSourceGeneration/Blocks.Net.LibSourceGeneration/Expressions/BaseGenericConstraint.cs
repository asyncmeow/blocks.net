using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public abstract class BaseGenericConstraint : IExpression
{
    public static implicit operator BaseGenericConstraint(string constraint) => new InjectedConstraint(constraint);

    public static implicit operator BaseGenericConstraint(TypeReference typeReference) =>
        new TypeConstraint(typeReference);
    
    public abstract string Generate();
}