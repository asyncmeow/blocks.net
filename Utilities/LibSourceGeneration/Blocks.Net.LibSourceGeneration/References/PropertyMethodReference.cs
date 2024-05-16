using System.Linq.Expressions;
using System.Text;
using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.Statements;

namespace Blocks.Net.LibSourceGeneration.References;

public class PropertyMethodReference(string methodType) : BaseBody<PropertyMethodReference>, IBuildable, IVisible<PropertyMethodReference>
{
    public List<IBuildable> Children = [];
    private bool _implicit;
    private VisibilityLevel _visibilityLevel = VisibilityLevel.Implicit;
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.AppendRepeating(indentation, indentationLevel).AppendVisibility(_visibilityLevel).Append(methodType);
        if (_implicit)
        {
            return builder.Append(";\n");
        }

        if (Children.Count == 1)
        {
            if (Children[0] is ExpressionStatement expressionStatement && methodType is "set" or "init" or "remove")
            {
                builder.Append(" => ");
                return expressionStatement.Build(builder,indentation,indentationLevel).Append(";\n");
            }

            if (Children[0] is ReturnStatement { Expression: not null } returnStatement && methodType is "get")
            {
                builder.Append(" => ");
                return returnStatement.Expression.Build(builder,indentation,indentationLevel).Append(";\n");
            }
        }
        
        builder.Append('\n').AppendRepeating(indentation, indentationLevel).Append("{\n");
        AppendChildren(builder, indentation, indentationLevel+1);
        return builder.AppendRepeating(indentation, indentationLevel).Append("}\n");
    }

    public PropertyMethodReference SetVisibility(VisibilityLevel visibilityLevel)
    {
        _visibilityLevel = visibilityLevel;
        return this;
    }

    public PropertyMethodReference Public() => SetVisibility(VisibilityLevel.Public);

    public PropertyMethodReference Internal() => SetVisibility(VisibilityLevel.Internal);

    public PropertyMethodReference Protected() => SetVisibility(VisibilityLevel.Protected);

    public PropertyMethodReference Private() => SetVisibility(VisibilityLevel.Private);

    public PropertyMethodReference ImplicitVisibility() => SetVisibility(VisibilityLevel.Implicit);

    public bool IsGetterWithSingleReturn =>
        methodType == "get" && Children.Count == 1 && Children[0] is ReturnStatement { Expression: not null };

    public override PropertyMethodReference From<T0>(Expression<T0> expression)
    {
        Children = [expression.ReturnType == typeof(void) ? expression.ToStatement() : new ReturnStatement(expression.ToExpression())];
        return this;
    }

    public PropertyMethodReference Implicit()
    {
        _implicit = true;
        return this;
    }
}