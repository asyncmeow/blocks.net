using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Statements;

public class ElseStatement : BaseBody<ElseStatement>, IBuildable
{
    public bool IsElseIf => Children.Count == 1 && Children[0] is IfStatement;
    public IfStatement ElseIf => (IfStatement)Children[0];
    
    
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.AppendRepeating(indentation, indentationLevel).Append("else");

        if (IsElseIf)
        {
            // builder.Append(" if\n").AppendRepeating(indentation,indentationLevel).Append("{\n");
            builder.Append(" if (");
            ElseIf.Condition.Build(builder, indentation, indentationLevel).Append(")\n")
                .AppendRepeating(indentation, indentationLevel).Append("{\n");
            
            AppendChildren(builder, indentation, indentationLevel + 1);
            builder.AppendRepeating(indentation, indentationLevel).Append("}\n");
            if (ElseIf.ElseStatement is { } elseStatement)
            {
                elseStatement.Build(builder, indentation, indentationLevel);
            }
        }
        else
        {
            builder.Append('\n').AppendRepeating(indentation, indentationLevel).Append("{\n");
            AppendChildren(builder, indentation, indentationLevel + 1);
            builder.AppendRepeating(indentation, indentationLevel).Append("}\n");
        }
        return builder;
    }
}