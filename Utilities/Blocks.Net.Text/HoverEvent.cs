using System.Text.Json.Nodes;
using Blocks.Net.Nbt;

namespace Blocks.Net.Text;

public abstract class HoverEvent
{
    public abstract JsonNode ToJson();
    public abstract NbtTag ToNbt();

    public static HoverEvent FromNbt(NbtTag tag)
    {
        var compound = (CompoundTag)tag;
        var action = ((StringTag)compound["action"]).Value;
        return action switch
        {
            "show_entity" => HoverEntity.FromNbt(compound),
            "show_item" => HoverItem.FromNbt(compound),
            "show_text" => HoverText.FromNbt(compound),
            _ => throw new Exception("Unknown hover action: " + action)
        };
    }

    public static implicit operator HoverEvent(string text) => new HoverText(text);
}