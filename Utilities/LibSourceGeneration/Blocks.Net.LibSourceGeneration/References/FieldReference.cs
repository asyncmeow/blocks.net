using System.Text;
using Blocks.Net.LibSourceGeneration.Builders;
using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Attribute = Blocks.Net.LibSourceGeneration.Expressions.Attribute;

namespace Blocks.Net.LibSourceGeneration.References;

public class FieldReference(TypeReference typeReference, string fieldName) : IBuildable, IAttributable<FieldReference>, IVisible<FieldReference>, IDocumentable<FieldReference>
{
    
    private VisibilityLevel _visibility = VisibilityLevel.Implicit;
    private List<Attribute> _attributes = [];
    private IExpression? _default = null;
    private DocCommentBuilder? _docCommentBuilder = null;
    private bool _static;
    private bool _readonly;
    private bool _event;
    
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        if (_docCommentBuilder != null)
        {
            var dc = _docCommentBuilder.Build();
            foreach (var line in dc)
            {
                builder.AppendRepeating(indentation, indentationLevel).Append("/// ").Append(line).Append('\n');
            }
        }

        foreach (var attribute in _attributes)
        {
            builder.AppendRepeating(indentation, indentationLevel).Append('[');
            attribute.Build(builder, indentation, indentationLevel).Append("]\n");
        }
        builder.AppendRepeating(indentation, indentationLevel).AppendVisibility(_visibility);
        if (_static) builder.Append("static ");
        if (_readonly) builder.Append("readonly ");
        if (_event) builder.Append("event ");
        builder.Append(typeReference.Generate()).Append(' ').Append(fieldName);
        if (_default != null)
        {
            builder.Append(" = ");
            _default.Build(builder, indentation, indentationLevel);
        }
        return builder.Append(";\n");
    }

    public FieldReference WithAttributes(params Attribute[] attributes)
    {
        _attributes.AddRange(attributes);
        return this;
    }

    public FieldReference WithAttributes(IEnumerable<Attribute> attributes)
    {
        _attributes.AddRange(attributes);
        return this;
    }

    public FieldReference SetVisibility(VisibilityLevel visibilityLevel)
    {
        _visibility = visibilityLevel;
        return this;
    }

    public FieldReference Public() => SetVisibility(VisibilityLevel.Public);
    public FieldReference Internal() => SetVisibility(VisibilityLevel.Internal);
    public FieldReference Protected() => SetVisibility(VisibilityLevel.Protected);
    public FieldReference Private() => SetVisibility(VisibilityLevel.Private);
    public FieldReference ImplicitVisibility() => SetVisibility(VisibilityLevel.Implicit);

    public FieldReference WithDocumentation(DocCommentBuilder docCommentBuilder)
    {
        _docCommentBuilder = docCommentBuilder;
        return this;
    }

    public FieldReference Static()
    {
        _static = true;
        return this;
    }

    public FieldReference Readonly()
    {
        _readonly = true;
        return this;
    }

    public FieldReference Event()
    {
        _event = true;
        return this;
    }

    public FieldReference Default(IExpression expression)
    {
        _default = expression;
        return this;
    }
    
}