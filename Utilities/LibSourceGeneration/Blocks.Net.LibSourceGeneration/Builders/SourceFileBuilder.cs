using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Builders;


/// <summary>
/// A tool used for generating source files in a structured manner, compared to using a string builder
/// 
/// </summary>
public class SourceFileBuilder : ITopLevelProvider<SourceFileBuilder>, IBuildable, ITypeProvider<SourceFileBuilder>
{
    private readonly List<string> _usings = [];
    private readonly List<string> _staticUsings = [];
    private Dictionary<string, string> _typeUsings = []; // This is used to generate aliasing for types, I'm not sure how needed it is
    
    // An optional string representing the file scoped namespaces
    private string? _fileScopedNamespace = null;

    public List<IBuildable> Children = [];

    
    /// <summary>
    /// Make a fully realized source file with all types and such resolved to exist in the source file
    /// </summary>
    /// <returns></returns>
    public string Build()
    {
        return Build(new StringBuilder(), "    ",0).ToString();
    }
    
    public SourceFileBuilder WithNamespace(string name, out NamespaceReference ns)
    {
        if (!string.IsNullOrEmpty(_fileScopedNamespace))
            throw new Exception("Cannot add a namespace to a file with a file scoped namespace!");
        ns = new NamespaceReference(name);
        Children.Add(ns);
        return this;
    }

    public SourceFileBuilder WithNamespace(string name, Action<NamespaceReference> construct)
    {
        if (!string.IsNullOrEmpty(_fileScopedNamespace))
            throw new Exception("Cannot add a namespace to a file with a file scoped namespace!");
        var ns = new NamespaceReference(name);
        construct(ns);
        Children.Add(ns);
        return this;
    }

    public SourceFileBuilder WithFileScopedNamespace(string name)
    {
        if (Children.OfType<NamespaceReference>().Any())
            throw new Exception("Cannot add a file scoped namespace to a file with child namespaces!");
        _fileScopedNamespace = name;
        return this;
    }
    

    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        foreach (var import in _usings)
        {
            builder.AppendRepeating(indentation, indentationLevel).Append($"using {import};\n");
        }

        foreach (var import in _staticUsings)
        {
            builder.AppendRepeating(indentation, indentationLevel).Append($"using static {import};\n");
        }

        foreach (var kv in _typeUsings)
        {
            builder.AppendRepeating(indentation, indentationLevel).Append($"using {kv.Key} = {kv.Value};\n");
        }

        if (!string.IsNullOrEmpty(_fileScopedNamespace))
        {
            builder.AppendRepeating(indentation, indentationLevel).Append($"namespace {_fileScopedNamespace};\n");
        }
        
        foreach (var child in Children)
        {
            child.Build(builder, indentation, indentationLevel);
        }
        return builder;
    }

    #region Type Provider
    public SourceFileBuilder AddClass(string name, out StructuredTypeReference @class)
    {
        @class = new StructuredTypeReference(name).AsClass();
        Children.Add(@class);
        return this;
    }

    public SourceFileBuilder AddClass(string name, Action<StructuredTypeReference> construct)
    {
        var @class = new StructuredTypeReference(name).AsClass();
        Children.Add(@class);
        construct(@class);
        return this;
    }

    public SourceFileBuilder AddStruct(string name, out StructuredTypeReference @struct)
    {
        @struct = new StructuredTypeReference(name).AsStruct();
        Children.Add(@struct);
        return this;
    }

    public SourceFileBuilder AddStruct(string name, Action<StructuredTypeReference> construct)
    {
        var @struct = new StructuredTypeReference(name).AsStruct();
        Children.Add(@struct);
        construct(@struct);
        return this;
    }

    public SourceFileBuilder AddRecord(string name, out StructuredTypeReference record)
    {
        record = new StructuredTypeReference(name).Record();
        Children.Add(record);
        return this;
    }

    public SourceFileBuilder AddRecord(string name, Action<StructuredTypeReference> construct)
    {
        var record = new StructuredTypeReference(name).Record();
        Children.Add(record);
        construct(record);
        return this;
    }

    public SourceFileBuilder AddInterface(string name, out StructuredTypeReference @interface)
    {
        @interface = new StructuredTypeReference(name).AsStruct();
        Children.Add(@interface);
        return this;
    }

    public SourceFileBuilder AddInterface(string name, Action<StructuredTypeReference> construct)
    {
        var @interface = new StructuredTypeReference(name).AsStruct();
        Children.Add(@interface);
        construct(@interface);
        return this;
    }
    #endregion
    
}