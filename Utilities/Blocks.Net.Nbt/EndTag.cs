namespace Blocks.Net.Nbt;

public class EndTag : NbtTag
{
    public override NbtTagType TagType => NbtTagType.End;
    public override string? Name { get; set; } = null;
    public override NbtTag[] Children => [];

    public override void WriteData(Stream stream)
    {
    }
}