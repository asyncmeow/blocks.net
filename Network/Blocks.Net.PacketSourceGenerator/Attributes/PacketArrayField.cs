namespace Blocks.Net.PacketSourceGenerator.Attributes;
/// <summary>
/// Used on arrays of primitive types, or structs, or fielded enums.
/// </summary>
/// <param name="arraySizeControl">An expression resulting in a value convertible to an int for the size of the array to read</param>
[AttributeUsage(AttributeTargets.Field)]
public class PacketArrayField(string arraySizeControl) : Attribute;