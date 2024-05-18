namespace Blocks.Net.PacketSourceGenerator.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class PacketSplitEnumDataField(string enumControl) : Attribute
{
    public string EnumControl => enumControl;
}