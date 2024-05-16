namespace Blocks.Net.LibSourceGeneration.Expressions;

public class InjectedConstraint(string injection) : BaseGenericConstraint
{
    public override string Generate() => injection;
}