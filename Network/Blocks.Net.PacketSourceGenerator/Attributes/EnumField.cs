namespace Blocks.Net.PacketSourceGenerator.Attributes;

/// <summary>
/// When used on a class, it means that this class corresponds to a field on a FieldedEnum
/// Which enum value it represents is determined by the class name with the enum name removed from the front
/// </summary>
/// <param name="enumeration"></param>
[AttributeUsage(AttributeTargets.Class)]
public class EnumField(Type enumeration) : Attribute
{
    public Type Enumeration => enumeration;
}