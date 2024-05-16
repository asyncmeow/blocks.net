using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class TypeConstraint(TypeReference t) : BaseGenericConstraint
{
    public override string Generate() => t;
}