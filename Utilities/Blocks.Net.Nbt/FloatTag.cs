using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Nbt;

public sealed class FloatTag : NbtTag
{
    public override NbtTagType TagType => NbtTagType.Short;
    public override string? Name { get; set; }
    public override NbtTag[] Children => [];
    public float Value;
    public static implicit operator float(FloatTag v) => v.Value;
    public FloatTag(Stream stream, bool readName = true)
    {
        Name = readName ? stream.ReadLengthPrefixedString() : null;
        var bytes = new byte[4];
        stream.ReadExactly(bytes);
        if (BitConverter.IsLittleEndian) bytes = [bytes[3], bytes[2], bytes[1], bytes[0]];
        Value = BitConverter.ToSingle(bytes);
        // Value = (sbyte)stream.ReadByte();
    }

    public FloatTag(string? name, float value)
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