using System.Linq.Expressions;
using Blocks.Net.LibSourceGeneration.Expressions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;
using Blocks.Net.LibSourceGeneration.Statements;

namespace Blocks.Net.LibSourceGeneration.Extensions;

public static class ExpressionExtensions
{
    public static IStatement ToStatement<T>(this Expression<T> expression) =>
        new ExpressionStatement(new InjectedExpression(expression.Body.ToString()));

    public static IExpression ToExpression<T>(this Expression<T> expression) =>
        new InjectedExpression(expression.Body.ToString());
    
    public static IEnumerable<ParameterReference> Parameters<T>(this Expression<T> expression) =>
        expression.Parameters.Select(parameter => new ParameterReference(parameter.Type,
            parameter.Name));
}