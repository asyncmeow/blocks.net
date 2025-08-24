using System.Reflection;
using Blocks.Net.Packets.Utilities;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;

[PublicAPI]
public readonly struct PrefixedOptional<T>(T? value) : IPrimitive where T : IPrimitive
{
    private static MethodInfo _readFromMethod = typeof(T).GetMethod("ReadFrom") ??
                                                throw new Exception($"Cannot find ReadFrom method on {typeof(T)}");

    public readonly T? Value = value;

    public PrefixedOptional() : this(default)
    {
    }

    public static implicit operator PrefixedOptional<T>(T? value) => new(value);
    public static implicit operator T?(PrefixedOptional<T> value) => value.Value;

    public void WriteTo(Stream stream)
    {
        if (Value == null)
        {
            new Boolean(false).WriteTo(stream);
        }
        else
        {
            new Boolean(true).WriteTo(stream);
            Value.WriteTo(stream);
        }
    }

    public static PrefixedOptional<T> ReadFrom(Stream stream) => Boolean.ReadFrom(stream)
        ? new PrefixedOptional<T>((T)_readFromMethod.Invoke(null, [stream])!)
        : new PrefixedOptional<T>();
}