namespace Blocks.Net.LibSourceGeneration.Constraints;

public class CombinedConstraint(params BaseGenericConstraint[] constraints) : BaseGenericConstraint
{
    public override string Generate() => string.Join(", ", constraints.Select(x => Generate()));
}