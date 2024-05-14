using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Nbt;

public interface INbtTag
{
    public NbtTagType TagType { get; }
    public string? Name { get; }
    public INbtTag[] Children { get; } // This is an empty array for anything other than Compound/List

    public void WriteNetwork(MemoryStream stream);
    public void Write(MemoryStream stream);

    public static INbtTag Read(MemoryStream stream)
    {
        var tag = (NbtTagType)stream.CheckedReadByte();
        switch (tag)
        {
            case NbtTagType.End:
                return new EndTag();
            case NbtTagType.Byte:
                break;
            case NbtTagType.Short:
                break;
            case NbtTagType.Int:
                break;
            case NbtTagType.Long:
                break;
            case NbtTagType.Float:
                break;
            case NbtTagType.Double:
                break;
            case NbtTagType.ByteArray:
                break;
            case NbtTagType.String:
                break;
            case NbtTagType.List:
                break;
            case NbtTagType.Compound:
                return new CompoundTag(stream);
            case NbtTagType.IntArray:
                break;
            case NbtTagType.LongArray:
                break;
            default:
                throw new Exception($"Unexpected tag: {tag}");
        }
    }

    public static INbtTag ReadNetwork(MemoryStream stream)
    {
        var tag = stream.CheckedReadByte();
        
    }
}