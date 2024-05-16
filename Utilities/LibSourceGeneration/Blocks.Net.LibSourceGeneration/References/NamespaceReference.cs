using System.Text;
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

    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        throw new NotImplementedException();
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
}