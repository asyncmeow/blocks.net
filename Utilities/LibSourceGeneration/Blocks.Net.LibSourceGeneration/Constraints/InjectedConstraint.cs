namespace Blocks.Net.LibSourceGeneration.Constraints;

public class InjectedConstraint(string injection) : BaseGenericConstraint
{
    public string Injection => injection;
    public override string Generate() => injection;
}