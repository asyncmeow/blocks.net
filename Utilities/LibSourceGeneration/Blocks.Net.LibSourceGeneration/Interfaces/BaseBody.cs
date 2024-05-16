using System.Linq.Expressions;
using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.References;
using Blocks.Net.LibSourceGeneration.Statements;

namespace Blocks.Net.LibSourceGeneration.Interfaces;

public abstract class BaseBody<T> : IBody<T> where T : BaseBody<T>
{
    public List<IBuildable> Children = [];

    public StringBuilder AppendChildren(StringBuilder builder, string indentation, int indentationLevel) =>
        Children.Aggregate(builder, (x, y) => y.Build(x, indentation, indentationLevel));
    
    public virtual T Add(IBuildable buildable)
    {
        Children.Add(buildable);
        return (T)this;
    }

    public virtual T Add(IExpression expression) => Add(new ExpressionStatement(expression));

    public virtual T Return(IExpression? value = null) => Add(new ReturnStatement(value));

    public virtual T DeclareVariable(TypeReference? t, string name, IExpression? init = null)
    {
        Children.Add(new VariableDeclarationStatement(t, name, init));
        return (T)this;
    }

    public virtual T DeclareVariable(string name, IExpression init)
    {
        Children.Add(new VariableDeclarationStatement(null, name, init));
        return (T)this;
    }

    public virtual T From<T0>(Expression<T0> expression)
    {
        Children = [expression.ToStatement()];
        return (T)this;
    }

    public virtual T Add<T0>(Expression<T0> expression)
    {
        
        Children.Add(expression.ToStatement());
        return (T)this;
    }

    public virtual T For(IStatement? decl, IExpression? compare, IExpression? increment, out ForLoopStatement forLoop)
    {
        forLoop = new ForLoopStatement(decl, compare, increment);
        Children.Add(forLoop);
        return (T)this;
    }

    public virtual T For(IStatement? decl, IExpression? compare, IExpression? increment, Action<ForLoopStatement> construct)
    {
        var forLoop = new ForLoopStatement(decl, compare, increment);
        construct(forLoop);
        Children.Add(forLoop);
        return (T)this;
    }

    public virtual T If(IExpression condition, out IfStatement statement)
    {
        statement = new IfStatement(condition);
        Children.Add(statement);
        return (T)this;
    }

    public virtual T If(IExpression condition, Action<IfStatement> construct)
    {
        var statement = new IfStatement(condition);
        construct(statement);
        Children.Add(statement);
        return (T)this;
    }
}