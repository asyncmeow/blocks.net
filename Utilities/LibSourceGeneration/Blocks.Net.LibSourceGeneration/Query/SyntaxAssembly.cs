using Microsoft.CodeAnalysis;

namespace Blocks.Net.LibSourceGeneration.Query;

/// <summary>
/// Treat a compilation as an "Assembly" for the purposes of getting types
/// </summary>
public class SyntaxAssembly
{
    public string? AssemblyName;
    public readonly SyntaxTree[] Trees;
    public SyntaxAssembly(GeneratorExecutionContext context)
    {
        var compilation = context.Compilation;
        AssemblyName = compilation.AssemblyName;
        Trees = compilation.SyntaxTrees.ToArray();
    }

    private List<SyntaxType>? _types;
    
    public IEnumerable<SyntaxType> Types
    {
        get
        {
            if (_types == null) ComputeTypes();
            return _types!;
        }
    }

    private List<SyntaxModule>? _modules;
    
    public IEnumerable<SyntaxModule> Modules
    {
        get
        {
            if (_modules == null) ComputeModules();
            return _modules!;
        }
    }

    private void ComputeModules()
    {
        _modules = [];
        foreach (var tree in Trees)
        {
            var module = SyntaxModule.From(this, tree);
            module.ComputeChildModules();
            _modules.Add(module);
        }
    }

    private void ComputeTypes()
    {
        _types = [];
        var modules = Modules;
        foreach (var module in modules)
        {
            _types.AddRange(module.GetTypes(true));
        }
    }
}