using System.Runtime.CompilerServices;
using System.Text;

namespace Blocks.Net.Packets.Primitives;

public struct Identifier(string ns, string name)
{
    public string Value => $"{ns}:{name}";
    public string Namespace => ns;
    public string Name => name;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator string(Identifier v) => v.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Identifier(string v)
    {
        var parts = v.Split(':');
        if (parts.Length == 1) return new("minecraft", parts[0]);
        return new(parts[0], parts[1]);
    }

    public void WriteTo(Stream stream)
    {
        ((String)Value).WriteTo(stream);
    }

    public static Identifier ReadFrom(MemoryStream stream)
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