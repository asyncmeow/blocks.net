using System.Text;
using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Nbt;

public abstract class NbtTag
{
    public abstract NbtTagType TagType { get; }
    public abstract string? Name { get; set; }
    public abstract NbtTag[] Children { get; } // This is an empty array for anything other than Compound/List

    public abstract void WriteData(Stream stream);

    public void Write(Stream stream, bool writeName=true)
    {
        stream.WriteByte((byte)TagType);
        if (writeName && TagType != NbtTagType.End && Name != null) stream.WriteLengthPrefixedString(Name!);
        WriteData(stream);
    }
    
    public static NbtTag Read(Stream stream, bool readName=true)
    {
        var tag = (NbtTagType)stream.CheckedReadByte();
        switch (tag)
        {
            case NbtTagType.End:
                return new EndTag();
            case NbtTagType.Byte:
                return new ByteTag(stream,readName);
            case NbtTagType.Short:
                return new ShortTag(stream, readName);
            case NbtTagType.Int:
                return new IntTag(stream, readName);
            case NbtTagType.Long:
                return new LongTag(stream, readName);
            case NbtTagType.Float:
                return new FloatTag(stream, readName);
            case NbtTagType.Double:
                return new DoubleTag(stream, readName);
            case NbtTagType.ByteArray:
                return new ByteArrayTag(stream, readName);
            case NbtTagType.String:
                return new StringTag(stream, readName);
            case NbtTagType.List:
                return new ListTag(stream, readName);
            case NbtTagType.Compound:
                return new CompoundTag(stream,readName);
            case NbtTagType.IntArray:
                return new IntArrayTag(stream, readName);
            case NbtTagType.LongArray:
                return new LongArrayTag(stream, readName);
            default:
                throw new Exception($"Unexpected tag: {tag}");
        }
    }

    protected abstract bool IsSameImpl(NbtTag other);
    
    public bool IsSameAs(NbtTag other, bool compareNames=false)
    {
        if (other.TagType != TagType) return false;
        if (compareNames && other.Name != Name) return false;
        return IsSameImpl(other);
    }

    public abstract void DumpImpl(StringBuilder sb, string indentation, int level, bool dumpName);
    public string Dump(string indentation="    ", bool dumpName=true)
    {
        var sb = new StringBuilder();
        DumpImpl(sb, indentation,0,dumpName);
        return sb.ToString();
    }
    
    // Now for some implicit operators meant for creating NBT tags quickly in a compound, or in lists (but you want to be explicit with typing for lists)
    // This is also the reason that compounds set tag names as well
    public static implicit operator NbtTag(sbyte value) => new ByteTag(null, value);
    public static implicit operator NbtTag(short value) => new ShortTag(null, value);
    public static implicit operator NbtTag(int value) => new IntTag(null, value);
    public static implicit operator NbtTag(long value) => new LongTag(null, value);
    public static implicit operator NbtTag(float value) => new FloatTag(null, value);
    public static implicit operator NbtTag(double value) => new DoubleTag(null, value);
    public static implicit operator NbtTag(string value) => new StringTag(null, value);
}