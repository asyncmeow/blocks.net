using System.Text;
using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Nbt;

public sealed class StringTag : NbtTag
{
    public override NbtTagType TagType => NbtTagType.String;
    public override string? Name { get; set; }
    public override NbtTag[] Children => [];

    public string Value;
    public static implicit operator string(StringTag s) => s.Value;

    public StringTag(Stream stream, bool readName)
    {
        Name = readName ? stream.ReadLengthPrefixedString() : null;
        Value = stream.ReadLengthPrefixedString();
    }

    public StringTag(string? name, string value)
    {
        Name = name;
        Value = value;
    }
    
    public override void WriteData(Stream stream)
    {
        stream.WriteLengthPrefixedString(Value);
    }

    protected override bool IsSameImpl(NbtTag other)
    {
        return ((StringTag)other).Value == Value;
    }

    public override void DumpImpl(StringBuilder sb, string indentation, int level, bool dumpName)
    {
        if (dumpName && Name != null)
        {
            sb.Append($"String({System.Web.HttpUtility.JavaScriptStringEncode(Name,true)}): ");
        }

        sb.Append(System.Web.HttpUtility.JavaScriptStringEncode(Value,true));
    }
}