using System.Text;

namespace Blocks.Net.Nbt;

public class EndTag : NbtTag
{
    public override NbtTagType TagType => NbtTagType.End;
    public override string? Name { get; set; } = null;
    public override NbtTag[] Children => [];

    public override void WriteData(Stream stream)
    {
    }

    protected override bool IsSameImpl(NbtTag other) => true;
    public override void DumpImpl(StringBuilder sb, string indentation, int level, bool dumpName)
    {
        sb.Append("End");
    }
}