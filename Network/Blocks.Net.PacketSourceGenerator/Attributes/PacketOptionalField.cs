namespace Blocks.Net.PacketSourceGenerator.Attributes;

/// <summary>
/// Sets this field as an optional field
/// </summary>
/// <param name="controllingCondition">An expression that results in a bool that determines if this optional is included, can use previously parsed fields</param>
public class PacketOptionalField(string controllingCondition) : Attribute;