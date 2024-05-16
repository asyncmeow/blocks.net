using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class ClassConstraint(bool nullable=false) : BaseGenericConstraint
{
    public override string Generate() => nullable ? "class?" : "class";
}