namespace Blocks.Net.LibSourceGeneration.Constraints;

public class CombinedConstraint(params BaseGenericConstraint[] constraints) : BaseGenericConstraint
{
    public IEnumerable<BaseGenericConstraint> Constraints => constraints;
    public override string Generate() => string.Join(", ", constraints.Select(x => Generate()));
}