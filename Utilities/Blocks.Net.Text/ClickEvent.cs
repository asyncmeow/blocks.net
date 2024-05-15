using Blocks.Net.Nbt;

namespace Blocks.Net.Text;

public class ClickEvent
{
    public string Action;
    public string Value;

    public string ToJson() =>
        $"{{\"action\":{System.Web.HttpUtility.JavaScriptStringEncode(Action, true)},\"value\":{System.Web.HttpUtility.JavaScriptStringEncode(Value, true)}}}";

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