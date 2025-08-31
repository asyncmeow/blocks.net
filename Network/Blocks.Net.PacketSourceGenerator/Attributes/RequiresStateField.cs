namespace Blocks.Net.PacketSourceGenerator.Attributes;

public class RequiresStateField(Type fieldType, string fieldName) : Attribute
{
    public Type FieldType = fieldType;
    public string FieldName =  fieldName;
    
}