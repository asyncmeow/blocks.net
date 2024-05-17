using System.Text;
using System.Text.RegularExpressions;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.Query;

namespace Blocks.Net.LibSourceGeneration.References;

// TODO: Switch this up with a *much* better type parsing system

public class TypeReference(string? ns, string name, bool isGeneric = false, params TypeReference?[] genericArgs)
    : IExpression
{
    public string? Namespace => ns;
    public string Name => name;

    public bool Generic => isGeneric;
    public TypeReference?[] GenericParameters => genericArgs;

    private List<int> _arrayRanks = [];

    public IEnumerable<int> Ranks => [];
    
    private bool _nullable = false;

    public bool Nullable => _nullable;
    
    public static implicit operator string(TypeReference typeReference)
    {
        var baseType = !string.IsNullOrEmpty(typeReference.Namespace)
            ? $"{typeReference.Namespace}.{typeReference.Name}"
            : typeReference.Name;
        if (typeReference.Generic)
            baseType =
                $"{baseType}<{string.Join(",", typeReference.GenericParameters.Select(x => x is not null ? (string)x : ""))}>";
        if (typeReference._nullable)
            baseType += "?";
        foreach (var rank in typeReference._arrayRanks)
        {
            var sb = new StringBuilder();
            sb.Append('[').AppendRepeating(",", rank - 1).Append(']');
            baseType += sb.ToString();
        }
        
        return baseType;
    }

    public static implicit operator TypeReference(Type t)
    {
        if (t is DummyType dummyType) return dummyType.Reference;
        List<int> ranks = [];
        while (t.IsArray)
        {
            ranks.Insert(0,t.GetArrayRank());
            t = t.GetElementType()!;
        }
        if (t.IsGenericTypeDefinition)
        {
            var name = t.Name.Substring(0, t.Name.IndexOf('`'));
            var numParams = int.Parse(t.Name.Substring(t.Name.IndexOf('`') + 1));
            var ns = t.Namespace;
            List<TypeReference?> empty = [];
            for (int i = 0; i < numParams; i++)
            {
                empty.Add(null);
            }
            var result = new TypeReference(ns, name, true, empty.ToArray())
            {
                _arrayRanks = ranks
            };
        }

        if (t.IsGenericType)
        {
            var name = t.Name.Substring(0, t.Name.IndexOf('`'));
            var ns = t.Namespace;
            var result = new TypeReference(ns, name, true,
                t.GenericTypeArguments.Select(param => param != null ? (TypeReference)param : null).ToArray())
                {
                    _arrayRanks = ranks
                };
        }

        TypeReference parsed = t.FullName;
        parsed._arrayRanks = ranks;
        return parsed;
    }

    public static explicit operator Type(TypeReference t) => new DummyType(t);
    

    public TypeReference WithGenericArgs(params TypeReference?[] args) => new(ns, name, true, args)
    {
        _arrayRanks = _arrayRanks,
        _nullable = _nullable
    };


    public TypeReference AsNullable() => new(ns, name, isGeneric, genericArgs)
    {
        _arrayRanks = _arrayRanks,
        _nullable = true
    };
    
    public TypeReference MakeArray(int rank) => new(ns, name, isGeneric, genericArgs)
    {
        _arrayRanks = _arrayRanks.Append(rank).ToList(),
        _nullable = _nullable
    };

    public TypeReference GetElementType() => new(ns, name, isGeneric, genericArgs)
    {
        _arrayRanks = _arrayRanks.Take(_arrayRanks.Count - 1).ToList(),
        _nullable = _nullable
    };

    public bool IsArray => _arrayRanks.Count > 0;
    
    private static TypeReference? ParseType(string type, ref int position)
    {
        if (type[position] == '>' || type[position] == ',')
        {
            return null;
        }

        List<string> chunks = [];
        string currentChunk = "";
        List<TypeReference?> generics = [];
        List<int> rank = [];
        bool optional = false;
        for (; position < type.Length; position++)
        {
            switch (type[position])
            {
                case '<':
                    position += 1;
                    while (type[position] != '>')
                    {
                        generics.Add(ParseType(type, ref position));
                        if (type[position] == ',') position++;
                    }
                    goto double_break;
                case '[':
                    var currentRank = 0;
                    do
                    {
                        currentRank++;
                        position += 1;
                    } while (type[position] != ']');
                    position += 1;
                    rank.Add(currentRank);
                    break;
                case '?':
                    optional = true;
                    break;
                case '.':
                    chunks.Add(currentChunk);
                    currentChunk = "";
                    break;
                case '>' or ',':
                    goto double_break;
                default:
                    currentChunk += type[position];
                    break;
            }
        }

        double_break:
        var name = string.IsNullOrEmpty(currentChunk) ? chunks.Last() : currentChunk;
        var ns = string.Join(".", string.IsNullOrEmpty(currentChunk) ? chunks.Take(chunks.Count - 1) : chunks);
        return new TypeReference(ns, name, generics.Count > 0, generics.ToArray())
        {
            _arrayRanks = rank,
            _nullable = optional
        };
    }
    

    private static readonly Regex Whitespace = new Regex(@"\s+");
    public static implicit operator TypeReference(string s)
    {
        var pos = 0;
        var type = ParseType(Whitespace.Replace(s,""), ref pos);
        return type!;
    }

    public override string ToString() => this;
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel) => builder.Append(this);

    public string Generate() => this;
    
    public static bool operator ==(TypeReference a, TypeReference b)
    {
        return a.ToString() == b.ToString();
    }

    public static bool operator !=(TypeReference a, TypeReference b)
    {
        return !(a == b);
    }
}