using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class NewObject(TypeReference? type=null, params IExpression[] arguments) : IExpression
{
    public CollectionInitializer? Initializer;
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.Append("new");
        if (type is not null)
        {
            builder.Append(' ').Append(type);
        }
        
        if (type is not null && arguments.Length == 0 && Initializer != null)
        {
            return BuildCollection(builder, indentation, indentationLevel).Append(';');
        }

        builder.Append('(').Join(", ",
            arguments.Select(x => x.Build(new StringBuilder(), indentation, indentationLevel).ToString())).Append(')');

        if (Initializer is not null)
        {
            BuildCollection(builder, indentation, indentationLevel);
        }

        return builder;
    }

    private StringBuilder BuildCollection(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.Append('\n').AppendRepeating(indentation, indentationLevel);
        return Initializer!.Build(builder, indentation, indentationLevel);
    }

    public NewObject InitializeWith(out CollectionInitializer initializer)
    {
        Initializer = initializer = new CollectionInitializer();
        return this;
    }

    public NewObject InitializeWith(Action<CollectionInitializer> construct)
    {
        Initializer = new CollectionInitializer();
        construct(Initializer);
        return this;
    }
}