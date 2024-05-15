using System.Text;
using System.Text.Json.Nodes;
using Blocks.Net.Nbt;

namespace Blocks.Net.Text;

public class HoverEntity(string type, Guid uuid, string? name = null) : HoverEvent
{
    public string Type => type;
    public Guid Uuid => uuid;
    public string? Name => name;
    public override JsonNode ToJson()
    {
        var content = new JsonObject
        {
            ["type"] = Type,
            ["uuid"] = Uuid.ToString(),
        };
        if (Name != null)
        {
            content["name"] = Name;
        }

        return new JsonObject
        {
            ["action"] = "show_entity",
            ["content"] = content
        };
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