using System.Text.Json.Nodes;
using Blocks.Net.Nbt;
using JetBrains.Annotations;

namespace Blocks.Net.Text;

[PublicAPI]
public class ClickEvent
{
    public string Action = "";
    public string Value = "";


    public JsonNode ToJson() => new JsonObject
    {
        ["action"] = Action,
        ["value"] = Value
    };

    public NbtTag ToNbtTag() => new CompoundTag
    {
        ["action"] = Action,
        ["value"] = Value
    };

    public static ClickEvent OpenUrl(string url) => new()
    {
        Action = "open_url",
        Value = url
    };

    public static ClickEvent RunCommand(string command) => new()
    {
        Action = "run_command",
        Value = command
    };

    public static ClickEvent SuggestCommand(string command) => new()
    {
        Action = "suggest_command",
        Value = command
    };

    public static ClickEvent ChangePage(int page) => new()
    {
        Action = "change_page",
        Value = page.ToString()
    };

    public static ClickEvent CopyToClipboard(string text) => new()
    {
        Action = "copy_to_clipboard",
        Value = text
    };
}