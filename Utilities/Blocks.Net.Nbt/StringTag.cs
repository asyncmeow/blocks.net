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
}