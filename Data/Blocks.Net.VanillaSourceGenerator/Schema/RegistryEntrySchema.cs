using Newtonsoft.Json;

namespace Blocks.Net.VanillaSourceGenerator;

public struct RegistryEntrySchema
{
    [JsonProperty("protocol_id")] public int ProtocolId;
}