namespace Blocks.Net.PacketSourceGenerator.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class PacketOptionalArrayField(string controllingCondition, string arraySizeControl, params string[] injectedArgs) : Attribute
{
    public string ControllingCondition => controllingCondition;
    public string ArraySizeControl => arraySizeControl;

    public IEnumerable<string> InjectedArgs => injectedArgs;
}