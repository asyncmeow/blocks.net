namespace Blocks.Net.PacketSourceGenerator.Attributes;

/// <summary>
/// Used on enum fields to determine the actual packet primitive type of the field
/// </summary>
/// <param name="EnumType">The primitive type of the enum</param>
[AttributeUsage(AttributeTargets.Field)]
public class PacketEnum(Type EnumType) : Attribute;