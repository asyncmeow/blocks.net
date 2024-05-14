namespace Blocks.Net.Nbt;

public class EndTag : INbtTag
{
    public NbtTagType TagType => NbtTagType.End;
    public string? Name => null;
    public INbtTag[] Children => [];
    public void WriteNetwork(MemoryStream stream)
    {
        stream.WriteByte((byte)TagType);
    }

    public void Write(MemoryStream stream)
    {
        stream.WriteByte((byte)TagType);
    }
}