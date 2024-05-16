using System.Linq.Expressions;
using System.Text;
using Blocks.Net.LibSourceGeneration.Builders;
using Blocks.Net.LibSourceGeneration.Constraints;
using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.Expressions;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Attribute = Blocks.Net.LibSourceGeneration.Expressions.Attribute;

namespace Blocks.Net.LibSourceGeneration.References;

/// <summary>
/// This is the class that contains information for records/structs/classes/interfaces
/// </summary>
/// <param name="name">The data type name</param>
public class StructuredTypeReference(string name)
    : ITypeProvider<StructuredTypeReference>, IBuildable, IType<StructuredTypeReference>
{
    private VisibilityLevel _visibility = VisibilityLevel.Implicit;
    private bool _partial = false;
    private bool _sealed = false;
    private bool _abstract = false;
    private bool _readonly = false;
    private bool _record = false;
    private bool _static = false;
    private StructureType _structureType = StructureType.Class;
    private DocCommentBuilder? _docCommentBuilder;
    private List<IBuildable> _children = [];
    private List<TypeParameterReference> _typeParameters = [];
    private List<ParameterReference> _primaryParameters = [];
    private List<Attribute> _attributes = [];
    private List<BaseType> _baseTypes = [];
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
        if (_readonly) builder.Append("readonly ");
        if (_sealed) builder.Append("sealed ");
        if (_abstract) builder.Append("abstract ");
        if (_partial) builder.Append("partial ");
        if (_record) builder.Append("record ");
        switch (_structureType)
        {
            case StructureType.Class when !_record:
                builder.Append("class ");
                break;
            case StructureType.Struct:
                builder.Append("struct ");
                break;
            case StructureType.Interface:
                builder.Append("interface ");
                break;
        }

        builder.Append(name);
        if (_typeParameters.Count > 0)
        {
            builder.Append('<').Append(string.Join(",", _typeParameters.Select(x => x.Build()))).Append('>');
        }

        if (_primaryParameters.Count > 0)
        {
            builder.Append('(').Append(string.Join(", ",
                    _primaryParameters.Select(x =>
                        x.Build(new StringBuilder(), indentation, indentationLevel).ToString())))
                .Append(')');
        }

        if (_baseTypes.Count > 0)
        {
            builder.Append(" : ").Join(", ",
                _baseTypes.Select(x => x.Build(new StringBuilder(), indentation, indentationLevel).ToString()));
        }

        foreach (var (paramName, constraint) in _constraints)
        {
            builder.Append(" where ").Append(paramName).Append(" : ").Append(constraint.Generate());
        }

        if (_children.Count == 0) return builder.Append(";\n");
        builder.Append('\n').AppendRepeating(indentation, indentationLevel).Append("{\n");
        foreach (var child in _children)
        {
            child.Build(builder, indentation, indentationLevel + 1);
        }

        return builder.AppendRepeating(indentation, indentationLevel).Append("}\n");
    }


    public StructuredTypeReference SetVisibility(VisibilityLevel visibilityLevel)
    {
        _visibility = visibilityLevel;
        return this;
    }

    public StructuredTypeReference Public() => SetVisibility(VisibilityLevel.Public);

    public StructuredTypeReference Internal() => SetVisibility(VisibilityLevel.Internal);

    public StructuredTypeReference Protected() => SetVisibility(VisibilityLevel.Protected);

    public StructuredTypeReference Private() => SetVisibility(VisibilityLevel.Private);

    public StructuredTypeReference ImplicitVisibility() => SetVisibility(VisibilityLevel.Implicit);

    public StructuredTypeReference Partial()
    {
        _partial = true;
        return this;
    }

    public StructuredTypeReference Sealed()
    {
        _sealed = true;
        return this;
    }

    public StructuredTypeReference Abstract()
    {
        _abstract = true;
        return this;
    }

    public StructuredTypeReference Readonly()
    {
        _readonly = true;
        return this;
    }

    public StructuredTypeReference Record()
    {
        _record = true;
        return this;
    }

    public StructuredTypeReference Static()
    {
        _static = true;
        return this;
    }

    public StructuredTypeReference AddField(TypeReference type, string name, out FieldReference field)
    {
        field = new FieldReference(type, name);
        _children.Add(field);
        return this;
    }

    public StructuredTypeReference AddField(TypeReference type, string name, Action<FieldReference> construct)
    {
        var field = new FieldReference(type, name);
        construct(field);
        _children.Add(field);
        return this;
    }

    public StructuredTypeReference AddProperty(TypeReference type, string name, out PropertyReference property)
    {
        property = new PropertyReference(type, name);
        _children.Add(property);
        return this;
    }

    public StructuredTypeReference AddProperty(TypeReference type, string name, Action<PropertyReference> construct)
    {
        var property = new PropertyReference(type, name);
        construct(property);
        _children.Add(property);
        return this;
    }

    public StructuredTypeReference AddConstructor(out MethodReference method)
    {
        method = new MethodReference(name);
        _children.Add(method);
        return this;
    }

    public StructuredTypeReference AddConstructor(Action<MethodReference> construct)
    {
        var method = new MethodReference(name);
        construct(method);
        _children.Add(method);
        return this;
    }

    public StructuredTypeReference AddDestructor(out MethodReference method)
    {
        method = new MethodReference($"~{name}");
        _children.Add(method);
        return this;
    }

    public StructuredTypeReference AddDestructor(Action<MethodReference> construct)
    {
        var method = new MethodReference($"~{name}");
        construct(method);
        _children.Add(method);
        return this;
    }

    public StructuredTypeReference AddConversionOperator(TypeReference to, out MethodReference method)
    {
        method = new MethodReference(to);
        _children.Add(method);
        return this;
    }

    public StructuredTypeReference AddConversionOperator(TypeReference to, Action<MethodReference> construct)
    {
        var method = new MethodReference(to);
        construct(method);
        _children.Add(method);
        return this;
    }

    public StructuredTypeReference AddMethod(TypeReference returnType, string name, out MethodReference method)
    {
        method = new MethodReference(returnType, name);
        _children.Add(method);
        return this;
    }

    public StructuredTypeReference AddMethod(TypeReference returnType, string name, Action<MethodReference> construct)
    {
        var method = new MethodReference(returnType, name);
        construct(method);
        _children.Add(method);
        return this;
    }

    public StructuredTypeReference AddStaticMethod<T0>(string name, Expression<T0> method, VisibilityLevel visibilityLevel=VisibilityLevel.Public)
    {
        var meth = new MethodReference(method.ReturnType, name);
        meth.Static().From(method).SetVisibility(visibilityLevel);
        _children.Add(meth);
        return this;
    }

    public StructuredTypeReference AsStruct()
    {
        _structureType = StructureType.Struct;
        return this;
    }

    public StructuredTypeReference AsInterface()
    {
        _structureType = StructureType.Interface;
        return this;
    }

    public StructuredTypeReference AsClass()
    {
        _structureType = StructureType.Class;
        return this;
    }

    public StructuredTypeReference WithDocumentation(DocCommentBuilder docCommentBuilder)
    {
        _docCommentBuilder = docCommentBuilder;
        return this;
    }

    public StructuredTypeReference WithAttributes(params Attribute[] attributes)
    {
        _attributes.AddRange(attributes);
        return this;
    }

    public StructuredTypeReference WithAttributes(IEnumerable<Attribute> attributes)
    {
        _attributes.AddRange(attributes);
        return this;
    }

    public StructuredTypeReference Inherit(params BaseType[] baseTypeReferences)
    {
        _baseTypes.AddRange(baseTypeReferences);
        return this;
    }

    public StructuredTypeReference WithPrimaryConstructorParameters(params ParameterReference[] parameterReferences)
    {
        _primaryParameters.AddRange(parameterReferences);
        return this;
    }


    public StructuredTypeReference WithGenericParameters(params TypeParameterReference[] names)
    {
        _typeParameters.AddRange(names);
        return this;
    }

    public StructuredTypeReference WithConstraint(string genericName, BaseGenericConstraint constraint)
    {
        _constraints.Add((genericName, constraint));
        return this;
    }

    
    
    #region Type Provider

    public StructuredTypeReference AddClass(string name, out StructuredTypeReference @class)
    {
        @class = new StructuredTypeReference(name).AsClass();
        _children.Add(@class);
        return this;
    }

    public StructuredTypeReference AddClass(string name, Action<StructuredTypeReference> construct)
    {
        var @class = new StructuredTypeReference(name).AsClass();
        _children.Add(@class);
        construct(@class);
        return this;
    }

    public StructuredTypeReference AddStruct(string name, out StructuredTypeReference @struct)
    {
        @struct = new StructuredTypeReference(name).AsStruct();
        _children.Add(@struct);
        return this;
    }

    public StructuredTypeReference AddStruct(string name, Action<StructuredTypeReference> construct)
    {
        var @struct = new StructuredTypeReference(name).AsStruct();
        _children.Add(@struct);
        construct(@struct);
        return this;
    }

    public StructuredTypeReference AddRecord(string name, out StructuredTypeReference record)
    {
        record = new StructuredTypeReference(name).Record();
        _children.Add(record);
        return this;
    }

    public StructuredTypeReference AddRecord(string name, Action<StructuredTypeReference> construct)
    {
        var record = new StructuredTypeReference(name).Record();
        _children.Add(record);
        construct(record);
        return this;
    }

    public StructuredTypeReference AddInterface(string name, out StructuredTypeReference @interface)
    {
        @interface = new StructuredTypeReference(name).AsStruct();
        _children.Add(@interface);
        return this;
    }

    public StructuredTypeReference AddInterface(string name, Action<StructuredTypeReference> construct)
    {
        var @interface = new StructuredTypeReference(name).AsStruct();
        _children.Add(@interface);
        construct(@interface);
        return this;
    }

    #endregion

    public StructuredTypeReference Add(IBuildable buildable)
    {
        _children.Add(buildable);
        return this;
    }
}