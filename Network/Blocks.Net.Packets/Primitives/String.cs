using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;

public readonly struct String(string v)
{
    public string Value => v;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator string(String v) => v.Value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator String(string v) => new(v);

    public void WriteTo(Stream stream)
    {
        var length = (VarInt)v.Length;
        length.WriteTo(stream);
        var bytes = Encoding.UTF8.GetBytes(v);
        stream.Write(bytes);
    }

    public static String ReadFrom(MemoryStream stream)
    {
        var length = (int)VarInt.ReadFrom(stream);
        using var reader = new BinaryReader(stream, Encoding.Unicode, true);
        var oldPosition = stream.Position;
        var readLength = Math.Min(3 * length, (int)(stream.Length - stream.Position));
        var bytes = reader.ReadBytes(readLength);
        var chars = Encoding.UTF8.GetChars(bytes);
        if (chars.Length < length) throw new Exception("Could not find enough UTF8 chars!");
        var str = new string(chars[..length]);
        var bc = Encoding.UTF8.GetByteCount(str);
        stream.Seek(oldPosition + bc, SeekOrigin.Begin);
        return str;
    }
}