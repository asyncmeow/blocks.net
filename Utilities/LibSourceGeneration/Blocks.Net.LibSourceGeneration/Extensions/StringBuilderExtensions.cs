using System.Text;
using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Extensions;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendRepeating(this StringBuilder sb, string toAppend, int repeatAmount = 1)
    {
        for (var i = 0; i < repeatAmount; i++)
        {
            sb.Append(toAppend);
        }
        return sb;
    }

    public static StringBuilder AppendVisibility(this StringBuilder sb, VisibilityLevel level) =>
        sb.Append(level switch
        {
            VisibilityLevel.Public => "public ",
            VisibilityLevel.Internal => "internal ",
            VisibilityLevel.Protected => "protected ",
            VisibilityLevel.Private => "private ",
            _ => ""
        });

    public static StringBuilder Join(this StringBuilder sb, string joiner, IEnumerable<string> toJoin)
    {
        var first = true;
        foreach (var toAppend in toJoin)
        {
            if (!first) sb.Append(joiner);
            else first = false;
            sb.Append(toAppend);
        }
        return sb;
    }
}