using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Nbt;

public class ShortTag : NbtTag
{
    public override NbtTagType TagType => NbtTagType.Short;
    public sealed override string? Name { get; set; }
    public override NbtTag[] Children => [];
    public short Value;
    public static implicit operator short(ShortTag v) => v.Value;
    public ShortTag(Stream stream, bool readName = true)
    {
        Name = readName ? stream.ReadLengthPrefixedString() : null;
        var bytes = new byte[2];
        stream.ReadExactly(bytes);
        if (BitConverter.IsLittleEndian) bytes = [bytes[1], bytes[0]];
        Value = BitConverter.ToInt16(bytes);
        // Value = (sbyte)stream.ReadByte();
    }

    public ShortTag(string? name, short value)
    {
        Name = name;
        Value = value;
    }

    public override void WriteData(Stream stream)
    {
        var bytes = BitConverter.GetBytes(Value);
        if (BitConverter.IsLittleEndian) bytes = [bytes[1], bytes[0]];
        stream.Write(bytes);
    }
}