namespace Blocks.Net.PacketSourceGenerator.Attributes;

public class GenerateXOrFor(Type otherType) : Attribute
{
    public Type OtherType =>  otherType;
}