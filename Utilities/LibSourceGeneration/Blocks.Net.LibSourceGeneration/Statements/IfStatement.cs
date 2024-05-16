using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Statements;

public class IfStatement(IExpression condition) : BaseBody<IfStatement>, IBuildable
{
    public ElseStatement? ElseStatement = null;
    public IExpression Condition => condition;
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.AppendRepeating(indentation,indentationLevel).Append("if (");
        Condition.Build(builder, indentation, indentationLevel).Append(")\n")
            .AppendRepeating(indentation, indentationLevel).Append("{\n");
            
        AppendChildren(builder, indentation, indentationLevel + 1);
        builder.AppendRepeating(indentation, indentationLevel).Append("}\n");
        if (ElseStatement is { } elseStatement)
        {
            elseStatement.Build(builder, indentation, indentationLevel);
        }
        return builder;
    }

    public IfStatement ElseIf(IExpression cond, out IfStatement elsif)
    {
        if (ElseStatement is { IsElseIf: true } elseStatement)
        {
            elseStatement.ElseIf.ElseIf(cond, out elsif);
            return this;
        }

        elsif = new IfStatement(cond);
        ElseStatement = new ElseStatement().Add(elsif);
        return this;
    }

    public IfStatement ElseIf(IExpression cond, Action<IfStatement> construct)
    {
        if (ElseStatement is { IsElseIf: true } elseStatement)
        {
            elseStatement.ElseIf.ElseIf(cond, construct);
            return this;
        }

        var elsif = new IfStatement(cond);
        construct(elsif);
        ElseStatement = new ElseStatement().Add(elsif);
        return this;
    }

    public IfStatement Else(out ElseStatement elseStatement)
    {
        if (ElseStatement is { IsElseIf: true } es)
        {
            es.ElseIf.Else(out elseStatement);
            return this;
        }

        elseStatement = new ElseStatement();
        ElseStatement = elseStatement;
        return this;
    }

    public IfStatement Else(Action<ElseStatement> construct)
    {
        if (ElseStatement is { IsElseIf: true } es)
        {
            es.ElseIf.Else(construct);
            return this;
        }
        var elseStatement = new ElseStatement();
        construct(elseStatement);
        ElseStatement = elseStatement;
        return this;
    }
}