namespace Blocks.Net.Packets.Utilities;

public static class StreamUtilities
{
    public static byte CheckedReadByte(this Stream stream)
    {
        var b = stream.ReadByte();
        if (b == -1) throw new Exception("Unexpected end of stream!"); // TODO: A better exception message here
        return (byte)b;
    }
}