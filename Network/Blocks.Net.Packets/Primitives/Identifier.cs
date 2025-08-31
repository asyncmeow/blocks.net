using System.Runtime.CompilerServices;
using System.Text;
using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Utilities;

namespace Blocks.Net.Packets.Primitives;

public struct Identifier(string ns, string name) : IPrimitive
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
    
    public static implicit operator Identifier(NamespacedIdentifier v) =>new(v.Namespace, v.Name);
    public static implicit operator NamespacedIdentifier(Identifier v) => new(v.Namespace, v.Name);
    
    public void WriteTo(Stream stream, PacketState state)
    {
        ((String)Value).WriteTo(stream, state);
    }

    public static Identifier ReadFrom(Stream stream, PacketState state)
    {
        var length = (int)VarInt.ReadFrom(stream, state);
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