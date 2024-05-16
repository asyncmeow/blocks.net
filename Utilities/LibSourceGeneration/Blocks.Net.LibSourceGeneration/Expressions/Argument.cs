using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class Argument(string name, IExpression value) : IExpression
{
    public string Generate() => $"{name}: {value.Generate()}";
}