using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class Assignment(IExpression lhs, IExpression rhs) : BinaryExpression(lhs,rhs)
{
    public override string Operator => "=";

    public Assignment(string lhs, IExpression rhs) : this(new InjectedExpression(lhs), rhs)
    {
        
    }
}