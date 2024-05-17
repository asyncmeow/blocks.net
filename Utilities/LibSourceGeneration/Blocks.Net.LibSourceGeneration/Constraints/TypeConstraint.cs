using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Constraints;

public class TypeConstraint(TypeReference t) : BaseGenericConstraint
{
    public TypeReference Type => t;
    public override string Generate() => t;
}