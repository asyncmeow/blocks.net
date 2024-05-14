using System.Text;

namespace Blocks.Net.Nbt.Utilities;

public static class StringBuilderUtilities
{
    public static StringBuilder AppendRepeating(this StringBuilder sb, string toAppend, int repeatAmount = 1)
    {
        for (var i = 0; i < repeatAmount; i++)
        {
            sb.Append(toAppend);
        }
        return sb;
    }
}