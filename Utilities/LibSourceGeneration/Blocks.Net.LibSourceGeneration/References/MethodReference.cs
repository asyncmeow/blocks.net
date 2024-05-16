using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Blocks.Net.LibSourceGeneration.Builders;
using Blocks.Net.LibSourceGeneration.Constraints;
using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.Expressions;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.Statements;
using Attribute = Blocks.Net.LibSourceGeneration.Expressions.Attribute;

namespace Blocks.Net.LibSourceGeneration.References;

/// <summary>
/// A reference to any method in a class (constructor/destructor/operators/interface methods/etc...)
/// </summary>
/// <param name="returnType">The type that this method returns</param>
/// <param name="name">The name of the method, null if it's a conversion operator or constructor/destructor</param>
public class MethodReference(
    TypeReference returnType,
    string? name=null) : BaseBody<MethodReference>, 
    IBuildable,
    IAttributable<MethodReference>,
    IVisible<MethodReference>,
    IDocumentable<MethodReference>,
    IGeneric<MethodReference>
{
    
    private VisibilityLevel _visibility = VisibilityLevel.Implicit;
    private bool _sealed = false;
    private bool _abstract = false;
    private bool _static = false;
    private bool _virtual = false;
    private bool _override = false;
    private bool _operator = false;
    private bool _implicit = false;
    private bool _explicit = false;
    private bool _withoutBody = false;
    private StructureType _structureType = StructureType.Class;
    private DocCommentBuilder? _docCommentBuilder;
    private List<IBuildable> Children = [];
    private List<TypeParameterReference> _typeParameters = [];
    private List<ParameterReference> _parameters = [];
    private List<BaseCall> _baseCalls = [];
    private List<Attribute> _attributes = [];
    private List<(string name, BaseGenericConstraint constraint)> _constraints = [];
    
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
        if (_implicit) builder.Append("implicit ");
        if (_explicit) builder.Append("explicit ");
        if (_operator) builder.Append("operator ");
        builder.Append((string)returnType);
        if (name != null) builder.Append(' ').Append(name);
        
        if (_typeParameters.Count > 0)
        {
            builder.Append('<').Append(string.Join(",", _typeParameters.Select(x => x.Build()))).Append('>');
        }
        
        builder.Append('(').Append(string.Join(", ", _parameters.Select(x => x.Build(new StringBuilder(),indentation,indentationLevel).ToString()))).Append(')');
        
        
        if (_baseCalls.Count > 0)
        {
            // builder.Append(" : ").Append(string.Join(", ", _baseCalls.Select(x => x.Generate())));
            builder.Append(" : ").Join(", ",
                _baseCalls.Select(x => x.Build(new StringBuilder(), indentation, indentationLevel).ToString()));
        }
        

        foreach (var (paramName, constraint) in _constraints)
        {
            builder.Append(" where ").Append(paramName).Append(" : ").Append(constraint.Generate());
        }
        
        if (_withoutBody)
        {
            return builder.Append(";\n");
        }

        builder.Append('\n').AppendRepeating(indentation, indentationLevel).Append("{\n");
        AppendChildren(builder, indentation, indentationLevel+1);
        return builder.AppendRepeating(indentation, indentationLevel).Append("}\n");
    }

    public override MethodReference From<T0>(Expression<T0> expression)
    {
        _parameters = expression.Parameters().ToList();
        _withoutBody = false;
        Children = [expression.ReturnType == typeof(void) ? expression.ToStatement() : new ReturnStatement(expression.ToExpression())];
        return this;
    }

    public override MethodReference Add<T0>(Expression<T0> expression)
    {
        _withoutBody = false;
        Children.Add(expression.ToStatement());
        return this;
    }

    public MethodReference WithAttributes(params Attribute[] attributes)
    {
        _attributes.AddRange(attributes);
        return this;
    }

    public MethodReference WithAttributes(IEnumerable<Attribute> attributes)
    {
        _attributes.AddRange(attributes);
        return this;
    }

    public MethodReference SetVisibility(VisibilityLevel visibilityLevel)
    {
        _visibility = visibilityLevel;
        return this;
    }

    public MethodReference Public() => SetVisibility(VisibilityLevel.Public);

    public MethodReference Internal() => SetVisibility(VisibilityLevel.Internal);

    public MethodReference Protected() => SetVisibility(VisibilityLevel.Protected);

    public MethodReference Private() => SetVisibility(VisibilityLevel.Private);

    public MethodReference ImplicitVisibility() => SetVisibility(VisibilityLevel.Implicit);

    public MethodReference WithDocumentation(DocCommentBuilder docCommentBuilder)
    {
        _docCommentBuilder = docCommentBuilder;
        return this;
    }

    public MethodReference WithParameters(params ParameterReference[] parameterReferences)
    {
        _parameters.AddRange(parameterReferences);
        return this;
    }

    public MethodReference WithGenericParameters(params TypeParameterReference[] names)
    {
        _typeParameters.AddRange(names);
        return this;
    }

    public MethodReference WithConstraint(string genericName, BaseGenericConstraint constraint)
    {
        _constraints.Add((genericName, constraint));
        return this;
    }

    public MethodReference Sealed()
    {
        _sealed = true;
        return this;
    }

    public MethodReference Abstract()
    {
        _abstract = true;
        return this;
    }

    public MethodReference Static()
    {
        _static = true;
        return this;
    }

    public MethodReference Virtual()
    {
        _virtual = true;
        return this;
    }

    public MethodReference Override()
    {
        _override = true;
        return this;
    }

    public MethodReference Operator()
    {
        _operator = true;
        return this;
    }

    public MethodReference Implicit()
    {
        _implicit = true;
        return this;
    }

    public MethodReference Explicit()
    {
        _explicit = true;
        return this;
    }

    public MethodReference WithoutImplementation()
    {
        _withoutBody = true;
        return this;
    }
}