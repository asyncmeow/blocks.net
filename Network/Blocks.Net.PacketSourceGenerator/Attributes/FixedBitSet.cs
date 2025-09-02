namespace Blocks.Net.PacketSourceGenerator.Attributes;


public class FixedBitSet(int indices) : Attribute
{
    public int Indices => indices;
}