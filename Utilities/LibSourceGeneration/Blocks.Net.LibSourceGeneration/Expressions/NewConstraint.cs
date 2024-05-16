namespace Blocks.Net.LibSourceGeneration.Expressions;

public class NewConstraint : BaseGenericConstraint
{
    public override string Generate() => "new()";
}