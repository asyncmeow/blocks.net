using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Blocks.Net.LibSourceGeneration.Query;

public class SyntaxPrimaryConstructor(SyntaxType type, ParameterListSyntax parameters)
{
    public IEnumerable<SyntaxParameter> Parameters => parameters.DescendantNodes().OfType<ParameterSyntax>()
        .Select(node => new SyntaxParameter(node, type.Module));
}