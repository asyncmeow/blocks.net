using Newtonsoft.Json;

namespace Blocks.Net.VanillaSourceGenerator;

public class RegistrySchema
{
    [JsonProperty("entries")] public Dictionary<string, RegistryEntrySchema> Entries;
    [JsonProperty("protocol_id")] public int ProtocolId;
}