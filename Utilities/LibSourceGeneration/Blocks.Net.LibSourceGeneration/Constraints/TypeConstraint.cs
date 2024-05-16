using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Constraints;

public class TypeConstraint(TypeReference t) : BaseGenericConstraint
{
    public override string Generate() => t;
}