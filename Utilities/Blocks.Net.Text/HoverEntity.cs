using System.Text;
using Blocks.Net.Nbt;

namespace Blocks.Net.Text;

public class HoverEntity(string type, Guid uuid, string? name = null) : HoverEvent
{
    public string Type => type;
    public Guid Uuid => uuid;
    public string? Name => name;
    public override string ToJson()
    {
        var sb = new StringBuilder();
        sb.Append(
            $"{{\"action\":\"show_entity\",\"content\":{{\"type\":{System.Web.HttpUtility.JavaScriptStringEncode(Type, true)}");
            sb.Append($",\"uuid\":{System.Web.HttpUtility.JavaScriptStringEncode(uuid.ToString(),true)}");
        if (Name != null)
        {
            sb.Append($",\"name\":{name}");
        }
        sb.Append("}}");
        return sb.ToString();
    }

    public override NbtTag ToNbt()
    {
        var content = new CompoundTag
        {
            ["type"] = Type,
            ["uuid"] = Uuid.ToString(),
        };
        if (Name != null)
        {
            content["name"] = Name;
        }

        var action = new CompoundTag
        {
            ["action"] = "show_entity",
            ["content"] = content
        };

        return action;
    }
}