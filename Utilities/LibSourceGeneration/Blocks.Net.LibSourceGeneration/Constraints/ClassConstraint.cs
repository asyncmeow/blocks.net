namespace Blocks.Net.LibSourceGeneration.Constraints;

public class ClassConstraint(bool nullable=false) : BaseGenericConstraint
{
    public override string Generate() => nullable ? "class?" : "class";
}