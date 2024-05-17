using Blocks.Net.LibSourceGeneration.Builders;
using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.References;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Blocks.Net.LibSourceGeneration.Query;

public sealed class SyntaxStruct(SyntaxModule module, string name, SyntaxType? parent = null) : SyntaxType(module, name, parent)
{
    public override SyntaxNode TypeRoot { get; } = null!;
    public override VisibilityLevel Visibility { get; }
    public override bool IsEnum => false;
    public override bool IsStruct => true;
    public override bool IsClass => false;
    public override bool IsInterface => false;
    public override bool IsStatic { get; }
    public override bool IsSealed { get; }
    public override bool IsPartial { get; }
    public override bool IsAbstract { get; }
    public override bool IsRecord { get; }
    public override SyntaxPrimaryConstructor? PrimaryConstructor { get; }
    public override StructuredTypeReference GenerateImplementation(SourceFileBuilder sourceFileBuilder)
    {
        if (!IsPartial) throw new Exception("Cannot generate the implementation for a non partial struct!");
        sourceFileBuilder.WithFileScopedNamespace(Module.Namespace).Using(module.Usings)
            .UsingStatic(module.StaticUsings);
        foreach (var (alias, original) in module.Aliases)
        {
            sourceFileBuilder.Alias(alias, original);
        }
        StructuredTypeReference t;
        if (parent != null)
        {
            var parentImpl = parent.GenerateImplementation(sourceFileBuilder);
            parentImpl.AddStruct(name, out t);
        }
        else
        {
            sourceFileBuilder.AddStruct(name, out t);
        }

        t.Partial();
        if (IsRecord)
        {
            t.Record();
        }

        if (IsStatic)
        {
            t.Static();
        }

        return t;
    }

    public SyntaxStruct(SyntaxModule module, TypeDeclarationSyntax declarationSyntax, SyntaxType? parent = null) : this(
        module, declarationSyntax.Identifier.ToString(), parent)
    {
        TypeRoot = declarationSyntax;
        Visibility = declarationSyntax.GetVisibility();
        IsStatic = declarationSyntax.HasToken(SyntaxKind.StaticKeyword);
        IsSealed = declarationSyntax.HasToken(SyntaxKind.SealedKeyword);
        IsPartial = declarationSyntax.HasToken(SyntaxKind.PartialKeyword);
        IsAbstract = declarationSyntax.HasToken(SyntaxKind.AbstractKeyword);
        if (declarationSyntax.ParameterList != null)
        {
            PrimaryConstructor = new SyntaxPrimaryConstructor(this, declarationSyntax.ParameterList);
        }
    }
    
    
    public SyntaxStruct(SyntaxModule module, StructDeclarationSyntax declarationSyntax, SyntaxType? parent = null) :
        this(module, (TypeDeclarationSyntax)declarationSyntax, parent) =>
        IsRecord = false;

    public SyntaxStruct(SyntaxModule module, RecordDeclarationSyntax declarationSyntax, SyntaxType? parent = null) :
        this(module, (TypeDeclarationSyntax)declarationSyntax, parent) =>
        IsRecord = true;
}