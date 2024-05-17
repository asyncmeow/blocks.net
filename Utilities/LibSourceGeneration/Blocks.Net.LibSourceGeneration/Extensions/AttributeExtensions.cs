using Blocks.Net.LibSourceGeneration.Query;
using Blocks.Net.LibSourceGeneration.References;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Blocks.Net.LibSourceGeneration.Extensions;

public static class AttributeExtensions
{
    private static readonly Dictionary<string, Type> TypeKeywords = new()
    {
        {"void", typeof(void)},
        {"byte",typeof(byte)},
        {"sbyte", typeof(sbyte)},
        {"short", typeof(short)},
        {"ushort", typeof(ushort)},
        {"int", typeof(int)},
        {"uint", typeof(uint)},
        {"long",typeof(long)},
        {"ulong",typeof(ulong)},
        {"float",typeof(float)},
        {"double",typeof(double)},
        {"decimal",typeof(decimal)},
        {"string", typeof(string)},
        {"char",typeof(char)},
        {"nint",typeof(nint)},
        {"nuint",typeof(nuint)}
    };
    
    /// <summary>
    /// Coerce an attribute syntax to an attribute
    /// Currently doesn't support named arguments, field arguments, or enum arguments
    /// Supports types, but uses a custom "DummyType" class for referring to them
    /// </summary>
    /// <param name="attributeSyntax">The attribute to coerce</param>
    /// <typeparam name="T">The attribute type to coerce to</typeparam>
    /// <returns>The attribute syntax coerced to the given attribute</returns>
    public static T CoerceTo<T>(this AttributeSyntax attributeSyntax) where T : Attribute
    {
        List<object> args = [];
        if (attributeSyntax.ArgumentList != null)
        {
            foreach (var arg in attributeSyntax.ArgumentList.DescendantNodes().OfType<AttributeArgumentSyntax>())
            {
                var expression = arg.Expression;
                switch (expression)
                {
                    case TypeOfExpressionSyntax typeOfExpressionSyntax:
                        var t = typeOfExpressionSyntax.Type.ToString();
                        if (TypeKeywords.TryGetValue(t, out var t2)) args.Add(t2);
                        else if (Type.GetType(t) is { } type) args.Add(type);
                        else args.Add(new DummyType(t));
                        break;
                    case LiteralExpressionSyntax literalExpressionSyntax:
                        args.Add(literalExpressionSyntax.Token.Value);
                        break;
                    default:
                        throw new Exception(
                            $"Cannot currently coerce expressions of type: {expression.Kind()} to attribute arguments!");
                }
            }
        }
        return (T)Activator.CreateInstance(typeof(T), args.ToArray());
    }
}