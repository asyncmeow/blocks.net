using Blocks.Net.LibSourceGeneration.Builders;
using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Blocks.Net.LibSourceGeneration.Query;

public abstract class SyntaxType(SyntaxModule module, string name, SyntaxType? parent = null) : IHasSyntaxAttributes
{
    public SyntaxModule Module => module;
    public SyntaxType? Parent => parent;
    /// <summary>
    /// This is the name without any class or namespace qualified names
    /// </summary>
    public string Name => name;
    
    public string ClassQualifiedName => Parent == null ? Name : $"{Parent.ClassQualifiedName}.{Name}";

    public string FullName => string.IsNullOrEmpty(Module.Namespace)
        ? ClassQualifiedName
        : $"{Module.Namespace}.{ClassQualifiedName}";

    public TypeReference LongReference => FullName;
    public TypeReference ShortReference => Name;
    
    public abstract SyntaxNode TypeRoot { get; }
    
    // All of this information can be different depending on the descendant types
    
    public abstract VisibilityLevel Visibility { get; }

    // This area is for determining what type of type this is
    public abstract bool IsEnum { get; }
    public abstract bool IsStruct { get; }
    public abstract bool IsClass { get; }
    public abstract bool IsInterface { get; }
    
    // Then the modifier on the type
    public abstract bool IsStatic { get; }
    public abstract bool IsSealed { get; }
    public abstract bool IsPartial { get; }
    public abstract bool IsAbstract { get; }
    public abstract bool IsRecord { get; }

    
    public abstract SyntaxPrimaryConstructor? PrimaryConstructor { get; }
    
    
    

    /// <summary>
    /// Create an implementation type for this type, will throw an exception if this type is not partial.
    /// </summary>
    /// <param name="sourceFileBuilder">The source file builder that will contain the implementation</param>
    /// <returns>The reference to the type to implement</returns>
    public abstract StructuredTypeReference GenerateImplementation(SourceFileBuilder sourceFileBuilder);

    public bool HasAttribute<T>() where T : Attribute => TypeRoot.HasAttribute<T>(Module);

    public IEnumerable<T> GetAttributes<T>() where T : Attribute
    {
        var fullName = typeof(T).FullName!;
        var toCheck = Module.GetAllCheckedNames(fullName).ToArray();
        var attributes = TypeRoot.DescendantNodes().OfType<AttributeSyntax>();
        foreach (var attribute in attributes)
        {
            if (toCheck.Contains(attribute.Name.ToString()))
            {
                yield return attribute.CoerceTo<T>();
            }
        }
    }

    public IEnumerable<SyntaxField> Fields =>
        TypeRoot.DescendantNodes()
            .OfType<FieldDeclarationSyntax>()
            .Select(decl => new SyntaxField(this,
                decl));

    public IEnumerable<SyntaxType> GetNestedTypes(bool recursive = false) => GetAllTypesIn(TypeRoot, module, recursive, this);

    public static IEnumerable<SyntaxType> GetAllTypesIn(SyntaxNode node, SyntaxModule module,  bool getNested=false, SyntaxType? parent = null)
    {
        var descendantNodes = node.DescendantNodes().ToArray();
        foreach (var structDecl in descendantNodes.OfType<StructDeclarationSyntax>())
        {
            var s = new SyntaxStruct(module, structDecl, parent);
            yield return s;
            if (!getNested) continue;
            foreach (var nested in s.GetNestedTypes(getNested)) yield return nested;
        }

        foreach (var classDecl in descendantNodes.OfType<ClassDeclarationSyntax>())
        {
            var c = new SyntaxClass(module, classDecl, parent);
            yield return c;
            if (!getNested) continue;
            foreach (var nested in c.GetNestedTypes(getNested)) yield return nested;
        }

        foreach (var interfaceDecl in descendantNodes.OfType<InterfaceDeclarationSyntax>())
        {
            var i = new SyntaxInterface(module, interfaceDecl, parent);
            yield return i;
            if (!getNested) continue;
            foreach (var nested in i.GetNestedTypes(getNested)) yield return nested;
        }

        foreach (var enumDecl in descendantNodes.OfType<EnumDeclarationSyntax>())
        {
            yield return new SyntaxEnum(module, enumDecl, parent);
        }
        
        foreach (var recordDecl in descendantNodes.OfType<RecordDeclarationSyntax>())
        {
            if (recordDecl.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword))
            {
                var s = new SyntaxStruct(module, recordDecl, parent);
                yield return s;
                if (!getNested) continue;
                foreach (var nested in s.GetNestedTypes(getNested)) yield return nested;
            }
            else
            {
                var c = new SyntaxClass(module, recordDecl, parent);
                yield return c;
                if (!getNested) continue;
                foreach (var nested in c.GetNestedTypes(getNested)) yield return nested;
            }
        }
    }
}