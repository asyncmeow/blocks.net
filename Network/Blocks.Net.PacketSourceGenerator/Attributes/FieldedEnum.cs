namespace Blocks.Net.PacketSourceGenerator.Attributes;


/// <summary>
/// Tells the code generator that this enum type represents a "FieldedEnum", and it will generate XImpl and IX classes for you
/// </summary>
/// <param name="primitiveType"></param>
[AttributeUsage(AttributeTargets.Enum)]
public class FieldedEnum(Type primitiveType) : Attribute;