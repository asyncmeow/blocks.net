using Newtonsoft.Json;

namespace Blocks.Net.PacketSourceGenerator.Schema;

public struct PacketSchema
{
    [JsonProperty("protocol_id")] public int ProtocolId;
}