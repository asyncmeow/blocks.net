using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Microsoft.CodeAnalysis;
using Attribute = Blocks.Net.LibSourceGeneration.Expressions.Attribute;

namespace Blocks.Net.LibSourceGeneration.References;

public class ParameterReference(TypeReference type, string name) : IAttributable<ParameterReference>, IBuildable
{
    public TypeReference Type => type;
    public string Name => name;
    public IExpression? Default => _default;
    
    // Why we would use this form rather than a completely different form
    public IEnumerable<Attribute> Attributes => _attributes;
    
    
    private IExpression? _default;
    private List<Attribute> _attributes = [];
    private bool _isVarArgs = false;
    private bool _isExtension = false;

    public ParameterReference WithDefault(IExpression def)
    {
        _default = def;
        return this;
    }

    public ParameterReference AsVarArgs()
    {
        _isVarArgs = true;
        return this;
    }

    public ParameterReference AsExtension()
    {
        _isExtension = true;
        return this;
    }

    public ParameterReference WithAttributes(params Attribute[] attributes)
    {
        _attributes.AddRange(attributes);
        return this;
    }

    public ParameterReference WithAttributes(IEnumerable<Attribute> attributes)
    {
        _attributes.AddRange(attributes);
        return this;
    }

    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        if (_attributes.Count > 0)
        {
            builder.Append('[').Join(", ",
                    _attributes.Select(x => x.Build(new StringBuilder(), indentation, indentationLevel + 1).ToString()))
                .Append("] ");
        }

        if (_isVarArgs)
        {
            builder.Append("params ");
        }

        if (_isExtension)
        {
            builder.Append("this ");
        }

        builder.Append(type).Append(' ').Append(name);
        if (_default != null)
        {
            builder.Append(" = ");
            _default.Build(builder, indentation, indentationLevel + 1);
        }

        return builder;
    }
}