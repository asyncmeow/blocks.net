using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.References;

public class NamespaceReference(string name) : ITopLevelProvider<NamespaceReference>, IBuildable, ITypeProvider<NamespaceReference>
{
    public string Name => name;
    
    
    
    public List<string> Usings = [];
    public List<string> StaticUsings = [];
    public Dictionary<string, string> TypeUsings = [];
    public List<IBuildable> Children = [];
    

    public NamespaceReference WithNamespace(string newName, out NamespaceReference ns)
    {
        ns = new NamespaceReference(name);
        Children.Add(ns);
        return this;
    }

    public NamespaceReference WithNamespace(string newName, Action<NamespaceReference> construct)
    {
        var ns = new NamespaceReference(name);
        construct(ns);
        Children.Add(ns);
        return this;
    }

    /// <inheritdoc/>
    public NamespaceReference Using(params string[] namespaces)
    {
        Usings.AddRange(namespaces);
        return this;
    }

    /// <inheritdoc/>
    public NamespaceReference Using(IEnumerable<string> namespaces)
    {
        Usings.AddRange(namespaces);
        return this;
    }

    /// <inheritdoc/>
    public NamespaceReference UsingStatic(params string[] classes)
    {
        StaticUsings.AddRange(classes);
        return this;
    }

    /// <inheritdoc/>
    public NamespaceReference UsingStatic(IEnumerable<string> classes)
    {
        StaticUsings.AddRange(classes);
        return this;
    }

    /// <inheritdoc/>
    public NamespaceReference Alias(string newTypeName, string oldTypeName)
    {
        TypeUsings[newTypeName] = oldTypeName;
        return this;
    }

    /// <inheritdoc/>
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.AppendRepeating(indentation, indentationLevel).Append("namespace ").Append(name).Append(" {\n");
        foreach (var import in Usings.Distinct())
        {
            builder.AppendRepeating(indentation, indentationLevel+1).Append($"using {import};\n");
        }

        foreach (var import in StaticUsings.Distinct())
        {
            builder.AppendRepeating(indentation, indentationLevel+1).Append($"using static {import};\n");
        }

        foreach (var kv in TypeUsings)
        {
            builder.AppendRepeating(indentation, indentationLevel+1).Append($"using {kv.Key} = {kv.Value};\n");
        }

        
        foreach (var child in Children)
        {
            child.Build(builder, indentation, indentationLevel+1);
        }

        return builder.Append("}\n");
    }

    #region Type Provider
    public NamespaceReference AddClass(string name, out StructuredTypeReference @class)
    {
        @class = new StructuredTypeReference(name).AsClass();
        Children.Add(@class);
        return this;
    }

    public NamespaceReference AddClass(string name, Action<StructuredTypeReference> construct)
    {
        var @class = new StructuredTypeReference(name).AsClass();
        Children.Add(@class);
        construct(@class);
        return this;
    }

    public NamespaceReference AddStruct(string name, out StructuredTypeReference @struct)
    {
        @struct = new StructuredTypeReference(name).AsStruct();
        Children.Add(@struct);
        return this;
    }

    public NamespaceReference AddStruct(string name, Action<StructuredTypeReference> construct)
    {
        var @struct = new StructuredTypeReference(name).AsStruct();
        Children.Add(@struct);
        construct(@struct);
        return this;
    }

    public NamespaceReference AddRecord(string name, out StructuredTypeReference record)
    {
        record = new StructuredTypeReference(name).Record();
        Children.Add(record);
        return this;
    }

    public NamespaceReference AddRecord(string name, Action<StructuredTypeReference> construct)
    {
        var record = new StructuredTypeReference(name).Record();
        Children.Add(record);
        construct(record);
        return this;
    }

    public NamespaceReference AddInterface(string name, out StructuredTypeReference @interface)
    {
        @interface = new StructuredTypeReference(name).AsStruct();
        Children.Add(@interface);
        return this;
    }

    public NamespaceReference AddInterface(string name, Action<StructuredTypeReference> construct)
    {
        var @interface = new StructuredTypeReference(name).AsStruct();
        Children.Add(@interface);
        construct(@interface);
        return this;
    }
    #endregion

    public NamespaceReference Add(IBuildable buildable)
    {
        Children.Add(buildable);
        return this;
    }
}