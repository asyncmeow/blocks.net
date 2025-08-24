using System.Reflection;
using Blocks.Net.Packets.Utilities;

namespace Blocks.Net.Packets.Primitives;

public readonly struct PrefixedArray<T>(T[] array) : IPrimitive where T : IPrimitive
{
    private static MethodInfo _readFromMethod = typeof(T).GetMethod("ReadFrom", [typeof(Stream)]) ??
                                                throw new Exception($"Cannot find ReadFrom method on {typeof(T)}");

    public T[] Array => array;
    public ref T this[int index] => ref array[index];

    public PrefixedArray() : this([]) { }
    
    public void WriteTo(Stream stream)
    {
        new VarInt(array.Length).WriteTo(stream);
        foreach (var value in array)
        {
            value.WriteTo(stream);
        }
    }

    public static PrefixedArray<T> ReadFrom(Stream stream)
    {
        int length = VarInt.ReadFrom(stream);
        var arr = new T[length];
        for (var i = 0; i < length; i++)
        {
            arr[i] = ((T)_readFromMethod.Invoke(null, [stream])!)!;
        }
        return new PrefixedArray<T>(arr);
    }
}