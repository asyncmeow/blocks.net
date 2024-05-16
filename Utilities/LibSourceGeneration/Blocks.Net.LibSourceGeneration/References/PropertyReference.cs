using System.Text;
using Blocks.Net.LibSourceGeneration.Builders;
using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.Statements;
using Attribute = Blocks.Net.LibSourceGeneration.Expressions.Attribute;

namespace Blocks.Net.LibSourceGeneration.References;

public class PropertyReference(TypeReference typeReference, string fieldName) : IBuildable, IAttributable<PropertyReference>, IVisible<PropertyReference>, IDocumentable<PropertyReference>, IAddable<PropertyReference>
{
    
    private VisibilityLevel _visibility = VisibilityLevel.Implicit;
    private List<Attribute> _attributes = [];
    private IExpression? _default = null;
    private DocCommentBuilder? _docCommentBuilder = null;
    private bool _static;
    private bool _event;
    private bool _abstract;
    private bool _override;
    private bool _sealed;
    private bool _virtual;

    private List<IBuildable> _children = [];
    
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
        if (_abstract) builder.Append("abstract ");
        if (_override) builder.Append("override ");
        if (_virtual) builder.Append("virtual ");
        if (_sealed) builder.Append("sealed ");
        if (_event) builder.Append("event ");
        builder.Append(typeReference.Generate()).Append(' ').Append(fieldName);
        if (_children.Count == 1 && _children[0] is PropertyMethodReference
            {
                IsGetterWithSingleReturn: true
            } propertyMethodReference)
        {
            var ret = ((ReturnStatement)propertyMethodReference.Children[0]).Expression!;
            builder.Append(" => ");
            return ret.Build(builder,indentation,indentationLevel).Append(";\n");
        }

        builder.Append('\n').AppendRepeating(indentation,indentationLevel).Append("{\n");

        foreach (var child in _children)
        {
            child.Build(builder, indentation, indentationLevel + 1);
        }
        
        builder.AppendRepeating(indentation,indentationLevel).Append("}");
        if (_default != null)
        {
            builder.Append(" = ");
            _default.Build(builder, indentation,indentationLevel).Append(';');
        }
        return builder.Append('\n');
    }

    public PropertyReference WithAttributes(params Attribute[] attributes)
    {
        _attributes.AddRange(attributes);
        return this;
    }
    
    public PropertyReference WithAttributes(IEnumerable<Attribute> attributes)
    {
        _attributes.AddRange(attributes);
        return this;
    }

    public PropertyReference SetVisibility(VisibilityLevel visibilityLevel)
    {
        _visibility = visibilityLevel;
        return this;
    }

    public PropertyReference Public() => SetVisibility(VisibilityLevel.Public);

    public PropertyReference Internal() => SetVisibility(VisibilityLevel.Internal);

    public PropertyReference Protected() => SetVisibility(VisibilityLevel.Protected);

    public PropertyReference Private() => SetVisibility(VisibilityLevel.Private);

    public PropertyReference ImplicitVisibility() => SetVisibility(VisibilityLevel.Implicit);

    public PropertyReference WithDocumentation(DocCommentBuilder docCommentBuilder)
    {
        _docCommentBuilder = docCommentBuilder;
        return this;
    }
    
    public PropertyReference Add(IBuildable buildable)
    {
        _children.Add(buildable);
        return this;
    }

    public PropertyReference AddGetter(out PropertyMethodReference getter)
    {
        getter = new PropertyMethodReference("get");
        _children.Add(getter);
        return this;
    }

    public PropertyReference AddGetter(Action<PropertyMethodReference> construct)
    {
        var getter = new PropertyMethodReference("get");
        construct(getter);
        _children.Add(getter);
        return this;
    }
    
    public PropertyReference AddSetter(out PropertyMethodReference setter)
    {
        setter = new PropertyMethodReference("set");
        _children.Add(setter);
        return this;
    }

    public PropertyReference AddSetter(Action<PropertyMethodReference> construct)
    {
        var setter = new PropertyMethodReference("set");
        construct(setter);
        _children.Add(setter);
        return this;
    }
    
    public PropertyReference AddInitializer(out PropertyMethodReference initializer)
    {
        initializer = new PropertyMethodReference("init");
        _children.Add(initializer);
        return this;
    }

    public PropertyReference AddInitializer(Action<PropertyMethodReference> construct)
    {
        var initializer = new PropertyMethodReference("init");
        construct(initializer);
        _children.Add(initializer);
        return this;
    }
    
    public PropertyReference AddAdder(out PropertyMethodReference adder)
    {
        adder = new PropertyMethodReference("add");
        _children.Add(adder);
        return this;
    }

    public PropertyReference AddAdder(Action<PropertyMethodReference> construct)
    {
        var adder = new PropertyMethodReference("add");
        construct(adder);
        _children.Add(adder);
        return this;
    }
    
    public PropertyReference AddRemover(out PropertyMethodReference remover)
    {
        remover = new PropertyMethodReference("remove");
        _children.Add(remover);
        return this;
    }

    public PropertyReference AddRemover(Action<PropertyMethodReference> construct)
    {
        var remover = new PropertyMethodReference("remove");
        construct(remover);
        _children.Add(remover);
        return this;
    }
    
    
    public PropertyReference Static()
    {
        _static = true;
        return this;
    }

    public PropertyReference Abstract()
    {
        _abstract = true;
        return this;
    }
    public PropertyReference Override()
    {
        _override = true;
        return this;
    }
    public PropertyReference Sealed()
    {
        _sealed = true;
        return this;
    }

    public PropertyReference Event()
    {
        _event = true;
        return this;
    }

    public PropertyReference Virtual()
    {
        _virtual = true;
        return this;
    }
}