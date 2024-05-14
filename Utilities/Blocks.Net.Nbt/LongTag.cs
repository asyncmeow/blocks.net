using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Nbt;

public sealed class LongTag : NbtTag
{
    public override NbtTagType TagType => NbtTagType.Short;
    public override string? Name { get; set; }
    public override NbtTag[] Children => [];
    public long Value;
    public static implicit operator long(LongTag v) => v.Value;
    public LongTag(Stream stream, bool readName = true)
    {
        Name = readName ? stream.ReadLengthPrefixedString() : null;
        var bytes = new byte[8];
        stream.ReadExactly(bytes);
        if (BitConverter.IsLittleEndian) bytes = [bytes[7], bytes[6], bytes[5], bytes[4], bytes[3], bytes[2], bytes[1], bytes[0]];
        Value = BitConverter.ToInt64(bytes);
        // Value = (sbyte)stream.ReadByte();
    }

    public LongTag(string? name, long value)
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
}