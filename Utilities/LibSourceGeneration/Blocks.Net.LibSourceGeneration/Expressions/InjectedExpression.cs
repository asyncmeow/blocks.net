using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class InjectedExpression(string injection) : IExpression
{
    public string Generate() => injection;
}