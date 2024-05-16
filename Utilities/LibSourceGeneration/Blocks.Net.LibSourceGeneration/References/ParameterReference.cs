using System.Text;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Microsoft.CodeAnalysis;

namespace Blocks.Net.LibSourceGeneration.References;

public class ParameterReference(TypeReference type, string name) : IAttributable<ParameterReference>
{
    public TypeReference Type => type;
    public string Name => name;
    public IExpression? Default;
    private List<Attribute> _attributes = [];
    private bool _isVarArgs = false;
    public ParameterReference WithDefault(IExpression def)
    {
        Default = def;
        return this;
    }

    public ParameterReference AsVarArgs()
    {
        _isVarArgs = true;
        return this;
    }
    public ParameterReference WithAttributes(params Attribute[] attributes)
    {
        _attributes.AddRange(attributes);
        return this;
    }
    
    public string Generate()
    {
        StringBuilder sb = new();
        if (_attributes.Count > 0)
        {
            sb.Append('[').Append(string.Join(", ", _attributes.Select(x => x.Generate()))).Append("] ");
        }

        if (_isVarArgs)
        {
            sb.Append("params ");
        }

        sb.Append(type).Append(' ').Append(name);
        if (Default != null)
        {
            sb.Append(" = ").Append(Default.Generate());
        }
        return sb.ToString();
    }
}