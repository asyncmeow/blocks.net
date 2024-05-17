using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Blocks.Net.LibSourceGeneration.Query;

public class SyntaxField(SyntaxType containingType, FieldDeclarationSyntax syntax) : IHasSyntaxAttributes
{
    public SyntaxModule Module => containingType.Module;
    public SyntaxType ContainingType => containingType;
    public FieldDeclarationSyntax Syntax => syntax;
    public string Type = syntax.Declaration.Type.ToString();

    public IEnumerable<(string name, SyntaxNode? @default)> Declarations =>
        syntax.Declaration.Variables.Select(decl => (decl.Identifier.ToString(), decl.Initializer?.Value))
            .Select(dummy => ((string name, SyntaxNode? @default))dummy);

    public bool HasMultiple => syntax.Declaration.Variables.Count > 1;

    public (string name, SyntaxNode? @default) Declaration => Declarations.First();
    public string Name => Declaration.name;
    public SyntaxNode? Default => Declaration.@default;

    public bool HasAttribute<T>() where T : Attribute => Syntax.HasAttribute<T>(Module);
    public IEnumerable<T> GetAttributes<T>() where T : Attribute => Syntax.GetAttributes<T>(Module);
}