using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.Query;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Blocks.Net.LibSourceGeneration.Extensions;

public static class SyntaxNodeExtensions
{
    public static bool HasToken(this SyntaxNode node, SyntaxKind kind)
    {
        return node.DescendantTokens().Any(x => x.IsKind(kind));
    }

    public static VisibilityLevel GetVisibility(this SyntaxNode node)
    {
        if (node.HasToken(SyntaxKind.PublicKeyword)) return VisibilityLevel.Public;
        if (node.HasToken(SyntaxKind.InternalKeyword)) return VisibilityLevel.Internal;
        if (node.HasToken(SyntaxKind.ProtectedKeyword)) return VisibilityLevel.Protected;
        if (node.HasToken(SyntaxKind.PrivateKeyword)) return VisibilityLevel.Private;
        return VisibilityLevel.Implicit;
    }
    
    public static bool HasAttribute<T>(this SyntaxNode node, SyntaxModule module) where T : Attribute
    {
        var fullName = typeof(T).FullName!;
        var toCheck = module.GetAllCheckedNames(fullName).ToArray();
        var attributes = node.DescendantNodes().OfType<AttributeSyntax>();

        return attributes.Any(attribute => toCheck.Contains(attribute.Name.ToString()));
    }
    
    public static IEnumerable<T> GetAttributes<T>(this SyntaxNode node, SyntaxModule module) where T : Attribute
    {
        var fullName = typeof(T).FullName!;
        var toCheck = module.GetAllCheckedNames(fullName).ToArray();
        var attributes = node.DescendantNodes().OfType<AttributeSyntax>();
        foreach (var attribute in attributes)
        {
            if (toCheck.Contains(attribute.Name.ToString()))
            {
                yield return attribute.CoerceTo<T>();
            }
        }
    }

}