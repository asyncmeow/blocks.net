using System.Text.Json.Nodes;
using Blocks.Net.Nbt;

namespace Blocks.Net.Text;

public abstract class HoverEvent
{
    public abstract JsonNode ToJson();
    public abstract NbtTag ToNbt();
}