using System.Numerics;
using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class CollectionInitializer : IExpression
{
    private List<IExpression> _initializers = [];
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        // We have to do this
        if (_initializers.Count == 0) return builder.Append("{}");
        builder.Append("{\n");
        var i = 0;
        foreach (var child in _initializers)
        {
            builder.AppendRepeating(indentation, indentationLevel + 1);
            child.Build(builder, indentation, indentationLevel + 1);
            if (++i != _initializers.Count) builder.Append(',');
            builder.Append('\n');
        }
        builder.AppendRepeating(indentation, indentationLevel).Append('}');
        return builder;
    }

    public CollectionInitializer Add(params IExpression[] expressions)
    {
        _initializers.AddRange(expressions);
        return this;
    }

    public CollectionInitializer SetField(string field, IExpression expression)
    {
        Add(new FieldInitializer(field, expression));
        return this;
    }
    
    // Shorthand for adding an certain types of initializers

    public IExpression this[BigInteger index]
    {
        set => Add(new SubscriptInitializer(new IntegerLiteral(index), value));
    }

    public IExpression this[string index]
    {
        set => Add(new SubscriptInitializer(new StringLiteral(index), value));
    }
}