using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class NotNullConstraint : BaseGenericConstraint
{
    public override string Generate() => "notnull";
}