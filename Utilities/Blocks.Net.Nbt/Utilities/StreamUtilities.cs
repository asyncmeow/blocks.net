using System.Text;

namespace Blocks.Net.Nbt.Utilities;

// TODO: Deduplicate this class into a separate assembly?
public static class StreamUtilities
{
    public static byte CheckedReadByte(this Stream stream)
    {
        var b = stream.ReadByte();
        if (b == -1) throw new Exception("Unexpected end of stream!"); // TODO: A better exception message here
        return (byte)b;
    }

    public static string ReadLengthPrefixedString(this Stream stream)
    {
        var bytes = new byte[2];
        stream.ReadExactly(bytes);
        if (BitConverter.IsLittleEndian) bytes = [bytes[1], bytes[0]];
        var length = BitConverter.ToInt16(bytes);
        var stringBytes = new byte[length];
        stream.ReadExactly(bytes);
        var data = Encoding.UTF8.GetString(stringBytes);
        return data;
    }

    public static void WriteLengthPrefixedString(this Stream stream, string str)
    {
        // var bytes = BitConverter.GetBytes((short)str.Length);
        // if (BitConverter.IsLittleEndian) bytes = [bytes[1], bytes[0]];
        var bytes = Encoding.UTF8.GetBytes(str);
        // Convert to Java's modified UTF-8 (where nulls are 0xC0, 0x80 instead of 0x00)
        bytes = bytes.SelectMany(x => x == 0x00 ? (byte[]) [0xC0, 0x80] : [x]).ToArray();
        var lenBytes = BitConverter.GetBytes((short)bytes.Length);
        if (BitConverter.IsLittleEndian) lenBytes = [lenBytes[1], lenBytes[0]];
        stream.Write(lenBytes);
        stream.Write(bytes);
    }
}