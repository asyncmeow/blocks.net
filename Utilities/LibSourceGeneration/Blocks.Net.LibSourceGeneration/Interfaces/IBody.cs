using System.Linq.Expressions;
using Blocks.Net.LibSourceGeneration.References;
using Blocks.Net.LibSourceGeneration.Statements;

namespace Blocks.Net.LibSourceGeneration.Interfaces;

public interface IBody<out T> : IAddable<T>
{
    public T Add(IExpression expression);

    // This just adds a return statement to this
    public T Return(IExpression? value);

    public T DeclareVariable(TypeReference? t, string name, IExpression? init=null);

    public T DeclareVariable(string name, IExpression init);

    public T From<T0>(Expression<T0> expression);

    public T Add<T0>(Expression<T0> expression);

    public T For(IStatement? decl, IExpression? compare, IExpression? increment, out ForLoopStatement forLoop);
    public T For(IStatement? decl, IExpression? compare, IExpression? increment, Action<ForLoopStatement> construct);

    public T If(IExpression condition, out IfStatement statement);

    public T If(IExpression condition, Action<IfStatement> construct);
}