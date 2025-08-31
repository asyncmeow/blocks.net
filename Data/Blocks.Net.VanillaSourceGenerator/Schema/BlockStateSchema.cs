
using Newtonsoft.Json;

namespace Blocks.Net.VanillaSourceGenerator;

public record BlockStateSchema
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("default")] public bool Default { get; set; } = false;
}