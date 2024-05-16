using System.Linq.Expressions;
using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Statements;

public class ForLoopStatement(IStatement? decl, IExpression? compare, IExpression? increment) : BaseBody<ForLoopStatement>, IStatement
{
    
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.AppendRepeating(indentation, indentationLevel).Append("for (");
        if (decl != null)
        {
            builder.Append('\n');
            decl.Build(builder, indentation, indentationLevel + 1);
        }
        else builder.Append(';');

        if (compare != null)
        {
            if (decl == null) builder.Append('\n');
            builder.AppendRepeating(indentation, indentationLevel + 1);
            compare.Build(builder, indentation, indentationLevel + 1).Append(";\n");
        }
        else builder.Append(';');

        if (increment != null)
        {
            if (decl == null && compare == null) builder.Append('\n');
            builder.AppendRepeating(indentation, indentationLevel + 1);
            increment.Build(builder, indentation, indentationLevel + 1).Append('\n');
        }

        if (decl != null || compare != null || increment != null)
            builder.AppendRepeating(indentation, indentationLevel);
        builder.Append(")\n").AppendRepeating(indentation, indentationLevel).Append("{\n");
        AppendChildren(builder, indentation, indentationLevel+1);
        return builder.AppendRepeating(indentation, indentationLevel).Append("}\n");
    }

}