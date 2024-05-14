using System.Text;
using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Nbt;

public sealed class IntArrayTag : NbtTag
{
    public override NbtTagType TagType => NbtTagType.IntArray;
    public override string? Name { get; set; }
    public override NbtTag[] Children => [];

    public readonly List<int> Data = [];
    
    public int this[int index]
    {
        get => Data[index];
        set => Data[index] = value;
    }

    // TODO: Add more utility methods as needed
    public void Clear() => Data.Clear();
    public void RemoveAt(int index) => Data.RemoveAt(index);
    public bool Remove(int value) => Data.Remove(value);
    public int RemoveAll(Predicate<int> match) => Data.RemoveAll(match);
    public void Add(int value) => Data.Add(value);
    public int Count => Data.Count;

    public IntArrayTag(Stream stream, bool readName)
    {
        Name = readName ? stream.ReadLengthPrefixedString() : null;
        var bytes = new byte[4];
        stream.ReadExactly(bytes);
        if (BitConverter.IsLittleEndian) bytes = [bytes[3], bytes[2], bytes[1], bytes[0]];
        var length = BitConverter.ToInt32(bytes);
        bytes = new byte[4];
        for (var i = 0; i < length; i++)
        {
            stream.ReadExactly(bytes);
            if (BitConverter.IsLittleEndian) bytes = [bytes[3], bytes[2], bytes[1], bytes[0]];
            Data.Add(BitConverter.ToInt32(bytes));
        }
    }
    
    public IntArrayTag(string? name, IEnumerable<int> data)
    {
        Name = name;
        Data = data.ToList();
    }
    
    public override void WriteData(Stream stream)
    {
        var bytes = BitConverter.GetBytes(Data.Count);
        if (BitConverter.IsLittleEndian) bytes = [bytes[3], bytes[2], bytes[1], bytes[0]];
        stream.Write(bytes);
        foreach (var value in Data)
        {
            bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) bytes = [bytes[3], bytes[2], bytes[1], bytes[0]];
            stream.Write(bytes);
        }
    }

    protected override bool IsSameImpl(NbtTag other)
    {
        var otherIa = (IntArrayTag)other;
        return otherIa.Data.SequenceEqual(Data);
    }
    
    public override void DumpImpl(StringBuilder sb, string indentation, int level, bool dumpName)
    {
        if (dumpName && Name != null)
        {
            sb.Append($"IntArray({System.Web.HttpUtility.JavaScriptStringEncode(Name,true)})");
        }
        else
        {
            sb.Append($"IntArray");
        }

        sb.Append(":\n");
        sb.AppendRepeating(indentation, level).Append("[\n");
        for (var i = 0; i < Count; i++)
        {
            sb.AppendRepeating(indentation, level + 1).Append($"{this[i]}");
            if (i != Count - 1) sb.Append(',');
            sb.Append('\n');
        }
        sb.AppendRepeating(indentation, level).Append(']');
    }
}