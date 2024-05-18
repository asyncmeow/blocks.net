namespace Blocks.Net.PacketSourceGenerator.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class PacketOptionalArrayField(string controllingCondition, string arraySizeControl) : Attribute
{
    public string ControllingCondition => controllingCondition;
    public string ArraySizeControl => arraySizeControl;
}