using System.Text;
using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Nbt;

public sealed class ByteTag : NbtTag
{
    public override NbtTagType TagType => NbtTagType.Byte;
    public override string? Name { get; set; }
    public override NbtTag[] Children => [];
    public sbyte Value;


    public static implicit operator sbyte(ByteTag v) => v.Value;
    
    public ByteTag(Stream stream, bool readName = true)
    {
        Name = readName ? stream.ReadLengthPrefixedString() : null;
        Value = (sbyte)stream.ReadByte();
    }

    public ByteTag(string? name, sbyte value)
    {
        Name = name;
        Value = value;
    }

    public override void WriteData(Stream stream)
    {
        stream.WriteByte((byte)Value);
    }

    protected override bool IsSameImpl(NbtTag other)
    {
        return ((ByteTag)other).Value == Value;
    }

    public override void DumpImpl(StringBuilder sb, string indentation, int level, bool dumpName)
    {
        if (dumpName && Name != null)
        {
            sb.Append($"Byte({System.Web.HttpUtility.JavaScriptStringEncode(Name,true)}): ");
        }
        sb.Append($"{Value}b");
    }
}