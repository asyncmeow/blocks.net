using Blocks.Net.LibSourceGeneration.Expressions;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Attribute = System.Attribute;

namespace Blocks.Net.LibSourceGeneration.Query;


public class SyntaxParameter(ParameterSyntax parameter, SyntaxModule module) : IHasSyntaxAttributes
{

    public ParameterSyntax Parameter => parameter;
    public SyntaxModule Module => module;

    public bool IsThis => Parameter.HasToken(SyntaxKind.ThisKeyword);
    public bool IsRef => Parameter.HasToken(SyntaxKind.RefKeyword);
    public bool IsIn => Parameter.HasToken(SyntaxKind.InKeyword);
    public bool IsOut => Parameter.HasToken(SyntaxKind.OutKeyword);
    public bool IsParams => Parameter.HasToken(SyntaxKind.ParamsKeyword);

    public string Name => parameter.Identifier.ToString();
    public string Type => parameter.Type!.ToString();

    public SyntaxNode? Default => parameter.Default?.Value;
    public static implicit operator ParameterReference(SyntaxParameter parameter)
    {
        var par = new ParameterReference(parameter.Type,parameter.Name);
        if (parameter.IsThis) par.AsExtension();
        if (parameter.IsParams) par.AsVarArgs();
        if (parameter.Default is { } @default) par.WithDefault(new InjectedExpression(@default.ToString()));
        return par;
    }

    public bool HasAttribute<T>() where T : Attribute => Parameter.HasAttribute<T>(Module);
    public IEnumerable<T> GetAttributes<T>() where T : Attribute => Parameter.GetAttributes<T>(Module);

}