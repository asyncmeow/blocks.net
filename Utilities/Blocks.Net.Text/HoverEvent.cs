using Blocks.Net.Nbt;

namespace Blocks.Net.Text;

public abstract class HoverEvent
{
    public abstract string ToJson();
    public abstract NbtTag ToNbt();
}