using System.Text;
using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Nbt;

public sealed class DoubleTag : NbtTag
{
    public override NbtTagType TagType => NbtTagType.Double;
    public override string? Name { get; set; }
    public override NbtTag[] Children => [];
    public double Value;
    public static implicit operator double(DoubleTag v) => v.Value;
    public DoubleTag(Stream stream, bool readName = true)
    {
        Name = readName ? stream.ReadLengthPrefixedString() : null;
        var bytes = new byte[8];
        stream.ReadExactly(bytes);
        if (BitConverter.IsLittleEndian) bytes = [bytes[7], bytes[6], bytes[5], bytes[4], bytes[3], bytes[2], bytes[1], bytes[0]];
        Value = BitConverter.ToDouble(bytes);
        // Value = (sbyte)stream.ReadByte();
    }

    public DoubleTag(string? name, double value)
    {
        Name = name;
        Value = value;
    }

    public override void WriteData(Stream stream)
    {
        var bytes = BitConverter.GetBytes(Value);
        if (BitConverter.IsLittleEndian) bytes = [bytes[7], bytes[6], bytes[5], bytes[4], bytes[3], bytes[2], bytes[1], bytes[0]];
        stream.Write(bytes);
    }
    
    
    protected override bool IsSameImpl(NbtTag other)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        return ((DoubleTag)other).Value == Value;
    }

    public override void DumpImpl(StringBuilder sb, string indentation, int level, bool dumpName)
    {
        if (dumpName && Name != null)
        {
            sb.Append($"Double({System.Web.HttpUtility.JavaScriptStringEncode(Name,true)}): ");
        }
        sb.Append($"{Value}d");
    }
}