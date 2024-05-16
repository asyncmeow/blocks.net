using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Blocks.Net.InjectorSourceGenerator;

[Generator]
public class InjectorGenerator : ISourceGenerator
{
    public const string Attribute =
        "/// <summary>\n/// Tell a source generator to embed a source file into a static field in a static constructor\n/// </summary>\n/// <param name=\"sourceName\">The short name of the file to embed (with the .cs)</param>\n[AttributeUsage(AttributeTargets.Field)]\npublic class EmbedMe(string sourceName) : Attribute;";

    public const string Attribute2 =
        "namespace Blocks.Net.InjectorSourceGenerator;\n\n/// <summary>\n/// Tell a source generator to embed all source files with a specific file scoped namespace into the parameter;\n/// </summary>\n/// <param name=\"namespaceName\">The namespace to embed (only does the top level of the namespace)</param>\n[AttributeUsage(AttributeTargets.Field)]\npublic class EmbedNamespace(string namespaceName) : Attribute;";
    public void Initialize(GeneratorInitializationContext context)
    {
    }


    public void Execute(GeneratorExecutionContext context)
    {
        context.AddSource("EmbedMe.g.cs", Attribute);
        context.AddSource("EmbedNamespace.g.cs", Attribute2);
        List<(string ns, string className, Dictionary<string, string>, Dictionary<string,string>)> injectedFiles = [];
        Dictionary<string, string> injectedFileValues = [];
        Dictionary<string, List<string>> injectedNamespaceValues = [];
        foreach (var tree in context.Compilation.SyntaxTrees)
        {
            var semanticModel = context.Compilation.GetSemanticModel(tree);
            var info = new FileInfo(tree.FilePath);
            var name = info.Name;
            var ns = tree.GetRoot().DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault()?.Name.ToString() ?? "";
            var txt =  System.Web.HttpUtility.JavaScriptStringEncode(tree.ToString(), true);
            injectedFileValues[name] = txt;
            if (!injectedNamespaceValues.TryGetValue(ns, out var namespaceInjection))
                namespaceInjection = injectedNamespaceValues[ns] = [];
            namespaceInjection.Add(txt);
            var classes = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var @class in classes)
            {
                Dictionary<string, string> localInjectedFiles = [];
                Dictionary<string, string> localInjectedNamespaces = [];
                foreach (var field in @class.DescendantNodes().OfType<FieldDeclarationSyntax>())
                {
                    var fieldAttrs = field.DescendantNodes().OfType<AttributeSyntax>().ToArray();
                    var decl = field.Declaration;
                    var fieldName = decl.Variables.First().Identifier.Text;
                    if (fieldAttrs.FirstOrDefault(a =>
                            a.DescendantTokens().Any(dt => CheckForEmbedMeAttribute(dt, semanticModel))) is
                        { } embedMeAttr)
                    {
                        
                        var descendantTokens = embedMeAttr.DescendantTokens().ToArray();
                        var stringNode = descendantTokens.LastOrDefault(dt => dt.IsKind(SyntaxKind.StringLiteralToken));
                        var stringValue = (string)stringNode.Value!;
                        localInjectedFiles[fieldName] = stringValue;
                    }
                    
                    if (fieldAttrs.FirstOrDefault(a =>
                            a.DescendantTokens().Any(dt => CheckForEmbedNamespaceAttribute(dt, semanticModel))) is
                        { } embedNsAttr)
                    {
                        
                        var descendantTokens = embedNsAttr.DescendantTokens().ToArray();
                        var stringNode = descendantTokens.LastOrDefault(dt => dt.IsKind(SyntaxKind.StringLiteralToken));
                        var stringValue = (string)stringNode.Value!;
                        localInjectedNamespaces[fieldName] = stringValue;
                    }
                }

                if (localInjectedFiles.Count > 0 || localInjectedNamespaces.Count > 0)
                {
                    injectedFiles.Add((ns, @class.Identifier.Text, localInjectedFiles, localInjectedNamespaces));
                }
            }
        }
        // Now let's build all our partial classes

        foreach (var (ns, clazz, fileInjections, namespaceInjections) in injectedFiles)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append("using Microsoft.CodeAnalysis;\n\n").Append($"namespace {ns};\n").Append($"partial class {clazz} {{\n");
            stringBuilder.Append($"    static {clazz}() {{\n");
            foreach (var injection in fileInjections)
            {
                var field = injection.Key;
                var value = injection.Value;
                var actualValue = injectedFileValues.TryGetValue(value, out var s)
                    ? s
                    : $"\"<-- ERROR: UNABLE TO INJECT {value} -->\"";
                stringBuilder.Append($"        {field} = {actualValue};\n");
            }

            foreach (var injection in namespaceInjections)
            {
                
                var field = injection.Key;
                var value = injection.Value;
                var actualValue = injectedNamespaceValues.TryGetValue(value, out var s)
                    ? s
                    : [];
                stringBuilder.Append($"        {field} = [\n");
                foreach (var file in actualValue)
                {
                    stringBuilder.Append($"            {file},\n");
                }
                stringBuilder.Append("        ];\n");
            }
            stringBuilder.Append("    }\n");
            stringBuilder.Append("    static void InjectAllEmbeddedSourceFiles(GeneratorExecutionContext context) {\n");

            foreach (var injection in fileInjections)
            {
                var field = injection.Key;
                var value = injection.Value;
                stringBuilder.Append($"        context.AddSource(\"{clazz}.{value.Replace(".cs", ".g.cs")}\",{field});\n");
            }

            foreach (var injection in namespaceInjections)
            {
                
                var field = injection.Key;
                var value = injection.Value;
                stringBuilder.Append($"        var {field}_index = 0;\n");
                stringBuilder.Append($"        foreach (var _value_ in {field}) {{\n");
                stringBuilder.Append(
                    $"            context.AddSource($\"{value}.{{{field}_index++}}.g.cs\",_value_);\n");
                stringBuilder.Append("        }\n");
            }

            stringBuilder.Append("    }\n");
            stringBuilder.Append("}\n");
            context.AddSource($"{ns}.{clazz}.g.cs", stringBuilder.ToString());
        }
    }
    private bool CheckForEmbedMeAttribute(SyntaxToken token, SemanticModel semanticModel)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        return name == "EmbedMe";
    }
    private bool CheckForEmbedNamespaceAttribute(SyntaxToken token, SemanticModel semanticModel)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return false;
        var name = semanticModel.GetTypeInfo(token.Parent!).Type?.Name;
        return name == "EmbedNamespace";
    }
}