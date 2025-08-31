using System.Runtime.CompilerServices;

namespace Blocks.Net.DataTypes;

public readonly struct NamespacedIdentifier(string ns, string name)
{
    public string Value => $"{ns}:{name}";
    public string Namespace => ns;
    public string Name => name;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator string(NamespacedIdentifier v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator NamespacedIdentifier(string v)
    {
        var parts = v.Split(':');
        if (parts.Length == 1) return new("minecraft", parts[0]);
        return new(parts[0], parts[1]);
    }
}