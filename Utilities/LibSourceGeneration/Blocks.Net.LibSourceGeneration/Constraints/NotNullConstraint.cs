namespace Blocks.Net.LibSourceGeneration.Constraints;

public class NotNullConstraint : BaseGenericConstraint
{
    public override string Generate() => "notnull";
}