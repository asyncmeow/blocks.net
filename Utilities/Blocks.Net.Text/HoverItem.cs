using System.Text;
using System.Text.Json.Nodes;
using Blocks.Net.Nbt;

namespace Blocks.Net.Text;

public class HoverItem(string id, int count=1,NbtTag? tag=null) : HoverEvent
{
    public string Id => id;
    public int Count => count;
    public NbtTag? Tag => tag;


    public override JsonNode ToJson()
    {
        var content = new JsonObject
        {
            ["id"] = id
        };
        if (count != 1)
        {
            content["count"] = count;
        }

        if (tag != null)
        {
            content["tag"] = tag.SNbt;
        }

        return new JsonObject
        {
            ["action"] = "show_item",
            ["content"] = content
        };
    }

    public override NbtTag ToNbt()
    {
        var content = new CompoundTag
        {
            ["id"] = id,
        };
        if (count != 1)
        {
            content["count"] = count;
        }
        if (tag != null)
        {
            content["tag"] = tag.SNbt;
        }
        var compound = new CompoundTag
        {
            ["action"] = "show_item",
            ["content"] = content
        };
        return compound;
    }
}