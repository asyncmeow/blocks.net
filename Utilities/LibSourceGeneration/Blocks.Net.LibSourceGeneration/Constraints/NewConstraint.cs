namespace Blocks.Net.LibSourceGeneration.Constraints;

public class NewConstraint : BaseGenericConstraint
{
    public override string Generate() => "new()";
}