using Blocks.Net.Nbt;

namespace Blocks.Net.Text;

public class HoverText(TextComponent toBeShown) : HoverEvent
{
    public TextComponent ToBeShown => toBeShown;

    public override string ToJson()
    {
        return $"{{\"action\":\"show_text\",\"content\":{toBeShown.ToJson()}}}";
    }

    public override NbtTag ToNbt()
    {
        return new CompoundTag
        {
            ["action"] = "show_text",
            ["content"] = toBeShown.ToNbt()
        };
    }
}