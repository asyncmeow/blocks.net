using System.Text;
using Blocks.Net.Nbt;

namespace Blocks.Net.Text;

public class HoverItem(string id, int count=1,NbtTag? tag=null) : HoverEvent
{
    public string Id => id;
    public int Count => count;
    public NbtTag? Tag => tag;


    public override string ToJson()
    {
        var sb = new StringBuilder();
        sb.Append(
            $"{{\"action\":\"show_item\",\"content\":{{\"id\":{System.Web.HttpUtility.JavaScriptStringEncode(id, true)}");
        if (count != 1)
        {
            sb.Append($",\"count\":{count}");
        }
        if (tag != null)
        {
            sb.Append($",\"tag\":{tag.SNbt}");
        }
        sb.Append("}}");
        return sb.ToString();
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