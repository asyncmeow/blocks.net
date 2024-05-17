using Blocks.Net.LibSourceGeneration.Builders;
using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.References;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Blocks.Net.LibSourceGeneration.Query;

public class SyntaxEnum(SyntaxModule module, string name, SyntaxType? parent = null) : SyntaxType(module, name, parent)
{
    public override SyntaxNode TypeRoot { get; } = null!;
    public override VisibilityLevel Visibility { get; }
    public override bool IsEnum => true;
    public override bool IsStruct => false;
    public override bool IsClass => false;
    public override bool IsInterface => false;
    public override bool IsStatic { get; }
    public override bool IsSealed { get; }
    public override bool IsPartial { get; }
    public override bool IsAbstract { get; }
    public override bool IsRecord => false;
    public override SyntaxPrimaryConstructor? PrimaryConstructor => null;
    public override StructuredTypeReference GenerateImplementation(SourceFileBuilder sourceFileBuilder)
    {
        throw new NotImplementedException();
    }

    public SyntaxEnum(SyntaxModule module, EnumDeclarationSyntax declarationSyntax, SyntaxType? parent = null) : this(
        module, declarationSyntax.Identifier.ToString(), parent)
    {
        TypeRoot = declarationSyntax;
        Visibility = declarationSyntax.GetVisibility();
        IsStatic = declarationSyntax.HasToken(SyntaxKind.StaticKeyword);
        IsSealed = declarationSyntax.HasToken(SyntaxKind.SealedKeyword);
        IsPartial = declarationSyntax.HasToken(SyntaxKind.PartialKeyword);
        IsAbstract = declarationSyntax.HasToken(SyntaxKind.AbstractKeyword);
    }
    
    // Enums will have a specific thing for getting their members
}