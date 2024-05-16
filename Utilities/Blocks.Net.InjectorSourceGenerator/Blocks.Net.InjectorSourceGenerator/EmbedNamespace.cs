namespace Blocks.Net.InjectorSourceGenerator;

/// <summary>
/// Tell a source generator to embed all source files with a specific file scoped namespace into the parameter;
/// </summary>
/// <param name="namespaceName">The namespace to embed (only does the top level of the namespace)</param>
[AttributeUsage(AttributeTargets.Field)]
public class EmbedNamespace(string namespaceName) : Attribute;