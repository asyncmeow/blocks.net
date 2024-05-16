using System.Text;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Constraints;

public abstract class BaseGenericConstraint : IExpression
{
    public static implicit operator BaseGenericConstraint(string constraint) => new InjectedConstraint(constraint);

    public static implicit operator BaseGenericConstraint(TypeReference typeReference) =>
        new TypeConstraint(typeReference);
    
    public abstract string Generate();
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        return builder.Append(Generate());
    }
}