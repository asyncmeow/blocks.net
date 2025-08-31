using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class Addition(IExpression lhs, IExpression rhs) : BinaryExpression(lhs, rhs)
{
    public override string Operator => "+";
}