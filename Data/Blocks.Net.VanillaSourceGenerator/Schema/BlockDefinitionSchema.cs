
using Newtonsoft.Json;

namespace Blocks.Net.VanillaSourceGenerator;

public record BlockDefinitionSchema
{
    [JsonProperty("type")] 
    public string Type { get; set; }
}