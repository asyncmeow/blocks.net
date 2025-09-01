using Newtonsoft.Json;

namespace Blocks.Net.PacketSourceGenerator.Schema;

public class ProtocolSchema
{
    [JsonProperty("clientbound")] public Dictionary<string, PacketSchema> ClientboundPacketIds;
    [JsonProperty("serverbound")] public Dictionary<string, PacketSchema> ServerboundPacketIds;
}