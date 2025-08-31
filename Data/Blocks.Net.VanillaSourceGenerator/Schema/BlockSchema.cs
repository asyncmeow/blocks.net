using System.Collections.Specialized;
using Newtonsoft.Json;

namespace Blocks.Net.VanillaSourceGenerator;

public record BlockSchema
{
    [JsonProperty("definition")] public BlockDefinitionSchema Definition { get; set; }
    [JsonProperty("properties")]
    public PropertySet Properties { get; set; } = new();
    [JsonProperty("states")] public BlockStateSchema[] States { get; set; }
}