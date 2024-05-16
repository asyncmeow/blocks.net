namespace Blocks.Net.LibSourceGeneration.Constraints;

public class InjectedConstraint(string injection) : BaseGenericConstraint
{
    public override string Generate() => injection;
}