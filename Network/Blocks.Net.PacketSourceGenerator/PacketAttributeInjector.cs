using Blocks.Net.InjectorSourceGenerator;
using Microsoft.CodeAnalysis;

namespace Blocks.Net.PacketSourceGenerator;

[Generator]
public partial class PacketAttributeInjector : ISourceGenerator
{
    [EmbedNamespace("Blocks.Net.PacketSourceGenerator.Attributes")]
    public static string[] Attributes;
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        InjectAllEmbeddedSourceFiles(context);
    }
}