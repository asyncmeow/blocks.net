using System.Text;
using System.Text.RegularExpressions;
using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Nbt;

public sealed class ByteArrayTag : NbtTag
{
    public override NbtTagType TagType => NbtTagType.ByteArray;
    public override string? Name { get; set; }
    public override NbtTag[] Children => [];

    public List<byte> Data;


    public sbyte this[int index]
    {
        get => (sbyte)Data[index];
        set => Data[index] = (byte)value;
    }

    // TODO: Add more utility methods as needed
    public void Clear() => Data.Clear();
    public void RemoveAt(int index) => Data.RemoveAt(index);
    public bool Remove(sbyte value) => Data.Remove((byte)value);
    public int RemoveAll(Predicate<sbyte> match) => Data.RemoveAll(x => match((sbyte)x));
    public void Add(sbyte value) => Data.Add((byte)value);
    public int Count => Data.Count;
    
    
    public ByteArrayTag(Stream stream, bool readName)
    {
        Name = readName ? stream.ReadLengthPrefixedString() : null;
        var bytes = new byte[4];
        stream.ReadExactly(bytes);
        if (BitConverter.IsLittleEndian) bytes = [bytes[3], bytes[2], bytes[1], bytes[0]];
        var length = BitConverter.ToInt32(bytes);
        var data = new byte[length];
        stream.ReadExactly(data);
        Data = data.ToList();
    }

    public ByteArrayTag(string? name, IEnumerable<sbyte> data)
    {
        Name = name;
        Data = data.Select(x => (byte)x).ToList();
    }
    
    public override void WriteData(Stream stream)
    {
        var bytes = BitConverter.GetBytes(Data.Count);
        if (BitConverter.IsLittleEndian) bytes = [bytes[3], bytes[2], bytes[1], bytes[0]];
        stream.Write(bytes);
        stream.Write(Data.ToArray());
    }

    protected override bool IsSameImpl(NbtTag other)
    {
        var otherBa = (ByteArrayTag)other;
        return otherBa.Data.SequenceEqual(Data);
    }

    public override void DumpImpl(StringBuilder sb, string indentation, int level, bool dumpName)
    {
        if (dumpName && Name != null)
        {
            sb.Append($"ByteArray({System.Web.HttpUtility.JavaScriptStringEncode(Name,true)})");
        }
        else
        {
            sb.Append($"ByteArray");
        }

        sb.Append(":\n");
        sb.AppendRepeating(indentation, level).Append("[\n");
        for (var i = 0; i < Count; i++)
        {
            sb.AppendRepeating(indentation, level + 1).Append($"{this[i]}b");
            if (i != Count - 1) sb.Append(',');
            sb.Append('\n');
        }
        sb.AppendRepeating(indentation, level).Append(']');
    }
}