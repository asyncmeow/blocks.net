namespace Blocks.Net.Packets.Utilities;

public interface IPrimitive
{
    public void WriteTo(Stream stream);
}