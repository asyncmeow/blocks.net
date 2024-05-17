using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class Equals(IExpression lhs, IExpression rhs) : BinaryExpression(lhs,rhs)
{
    public override string Operator => "==";
}