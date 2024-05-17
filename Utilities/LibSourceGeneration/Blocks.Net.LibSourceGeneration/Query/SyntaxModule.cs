using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Blocks.Net.LibSourceGeneration.Query;

public class SyntaxModule(SyntaxAssembly assembly, SyntaxNode root)
{
    public SyntaxAssembly Assembly => assembly;
    public string Namespace = "";
    public List<string> Usings = [];
    public List<string> StaticUsings = [];
    public List<(string alias, string original)> Aliases = [];
    public List<SyntaxModule> Children = [];
    public SyntaxNode Root => root;

    public static SyntaxModule From(SyntaxAssembly assembly, SyntaxTree tree)
    {
        var root = tree.GetRoot();
        var module = new SyntaxModule(assembly, root);
        var nodes = root.DescendantNodes().ToArray();
        foreach (var decl in nodes.OfType<UsingDirectiveSyntax>())
        {
            if (decl.StaticKeyword != default)
            {
                module.StaticUsings.Add(decl.Name!.ToString());
            } else if (decl.Alias is { } alias)
            {
                module.Aliases.Add(decl.Name == null
                    ? (alias.Name.ToString(), decl.NamespaceOrType.ToString())
                    : (alias.Name.ToString(), decl.Name.ToString()));
            }
            else
            {
                module.Usings.Add(decl.Name!.ToString());
            }
        }

        if (root.DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault() is
            { } declarationSyntax)
        {
            module.Namespace = declarationSyntax.Name.ToString();
        }

        return module;
    }

    private static SyntaxModule From(SyntaxModule parent, NamespaceDeclarationSyntax declaration)
    {
        var module = new SyntaxModule(parent.Assembly, declaration)
        {
            Namespace = string.IsNullOrEmpty(parent.Namespace)
                ? declaration.Name.ToString()
                : $"{parent.Namespace}.{declaration.Name.ToString()}",
            Usings = parent.Usings.ToList(),
            StaticUsings = parent.StaticUsings.ToList(),
            Aliases = parent.Aliases.ToList()
        };
        
        // Get usings
        var nodes = declaration.DescendantNodes().ToArray();
        foreach (var decl in nodes.OfType<UsingDirectiveSyntax>())
        {
            if (decl.StaticKeyword != default)
            {
                module.StaticUsings.Add(decl.Name!.ToString());
            } else if (decl.Alias is { } alias)
            {
                module.Aliases.Add(decl.Name == null
                    ? (alias.Name.ToString(), decl.NamespaceOrType.ToString())
                    : (alias.Name.ToString(), decl.Name.ToString()));
            }
            else
            {
                module.Usings.Add(decl.Name!.ToString());
            }
        }

        return module;
    }

    /// <summary>
    /// This is used to compute the child modules, it is recursive by default
    /// </summary>
    public void ComputeChildModules()
    {
        foreach (var decl in root.DescendantNodes().OfType<NamespaceDeclarationSyntax>())
        {
            var newChild = From(this,decl);
            newChild.ComputeChildModules();
            Children.Add(newChild);
        }
    }

    public IEnumerable<SyntaxType> GetTypes(bool getNested = false) => SyntaxType.GetAllTypesIn(root, this, getNested);
    public IEnumerable<string> GetAllCheckedNames(string fullName)
    {
        List<string> allCheckedNamesNoAliases = [fullName];
        yield return fullName;
        if (!string.IsNullOrEmpty(Namespace))
        {
            var parts = Namespace.Split('.');
            var current = parts.First();
            if (fullName.StartsWith($"{current}."))
            {
                var subName = fullName.Substring($"{current}.".Length);
                yield return subName;
                allCheckedNamesNoAliases.Add(subName);
            }
            for (int i = 1; i < parts.Length; i++)
            {
                current = $"{current}.{parts[i]}";
                if (fullName.StartsWith($"{current}."))
                {
                    var subName = fullName.Substring($"{current}.".Length);
                    yield return subName;
                    allCheckedNamesNoAliases.Add(subName);
                }
            }
        }
        foreach (var use in Usings)
        {
            if (fullName.StartsWith($"{use}."))
            {
                var subName = fullName.Substring($"{use}.".Length);
                yield return subName;
                allCheckedNamesNoAliases.Add(subName);
            }
        }

        foreach (var (alias, original) in Aliases)
        {
            if (allCheckedNamesNoAliases.Contains(original))
            {
                yield return alias;
            }
        }
    }
}