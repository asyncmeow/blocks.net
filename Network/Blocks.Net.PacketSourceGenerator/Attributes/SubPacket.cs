namespace Blocks.Net.PacketSourceGenerator.Attributes;

/// <summary>
/// Generate .ReadFrom and .WriteTo methods for this class like it were some form of sub packet
/// (Mostly used for generating new primitive types based on structured data)
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class SubPacket : Attribute;