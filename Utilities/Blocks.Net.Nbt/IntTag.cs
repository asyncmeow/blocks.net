using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Nbt;

public sealed class IntTag : NbtTag
{
    public override NbtTagType TagType => NbtTagType.Int;
    public override string? Name { get; set; }
    public override NbtTag[] Children => [];
    public int Value;
    public static implicit operator int(IntTag v) => v.Value;
    public IntTag(Stream stream, bool readName = true)
    {
        Name = readName ? stream.ReadLengthPrefixedString() : null;
        var bytes = new byte[4];
        stream.ReadExactly(bytes);
        if (BitConverter.IsLittleEndian) bytes = [bytes[3], bytes[2], bytes[1], bytes[0]];
        Value = BitConverter.ToInt32(bytes);
        // Value = (sbyte)stream.ReadByte();
    }

    public IntTag(string? name, int value)
    {
        Name = name;
        Value = value;
    }

    public override void WriteData(Stream stream)
    {
        var bytes = BitConverter.GetBytes(Value);
        if (BitConverter.IsLittleEndian) bytes = [bytes[3], bytes[2], bytes[1], bytes[0]];
        stream.Write(bytes);
    }
}