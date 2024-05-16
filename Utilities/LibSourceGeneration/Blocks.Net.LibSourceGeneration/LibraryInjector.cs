using Blocks.Net.InjectorSourceGenerator;
using Microsoft.CodeAnalysis;

namespace Blocks.Net.LibSourceGeneration;

[Generator]
public partial class LibraryInjector : ISourceGenerator
{
    [EmbedNamespace("Blocks.Net.LibSourceGeneration.Builders")]
    public static string[] Builders;
    [EmbedNamespace("Blocks.Net.LibSourceGeneration.Definitions")]
    public static string[] Definitions;
    [EmbedNamespace("Blocks.Net.LibSourceGeneration.Expressions")]
    public static string[] Expressions;
    [EmbedNamespace("Blocks.Net.LibSourceGeneration.Extensions")]
    public static string[] Extensions;
    [EmbedNamespace("Blocks.Net.LibSourceGeneration.Interfaces")]
    public static string[] Interfaces;
    [EmbedNamespace("Blocks.Net.LibSourceGeneration.References")]
    public static string[] References;
    [EmbedNamespace("Blocks.Net.LibSourceGeneration.Statements")]
    public static string[] Statements;

    [EmbedNamespace("Blocks.Net.LibSourceGeneration.Constraints")]
    public static string[] Constraints;
    
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        InjectAllEmbeddedSourceFiles(context);
    }
}