namespace Blocks.Net.PacketSourceGenerator.Attributes;

/// <summary>
/// Marks this field as a field for a packet
/// The type of the field must be either in the Blocks.Net.Packets.Primitives namespace or have a static ReadFrom(MemoryStream) method
/// And a static WriteTo(MemoryStream) method.
///
/// If the type of the field is an enum it is assumed to be a Primitives.VarInt for parsing purposes
///
/// Other conversions are as follows for parsing classes that are implicitly converted to the field and reverse
/// bool   ->  Primitives.Boolean
/// byte   ->  Primitives.UnsignedByte
/// sbyte  ->  Primitives.Byte
/// short  ->  Primitives.Short
/// ushort ->  Primitives.UnsignedShort
/// int    ->  Primitives.Int
/// long   ->  Primitives.Long
/// float  ->  Primitives.Float
/// double ->  Primitives.Double
/// string ->  Primitives.String
///
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class PacketField(params string[] injectedArguments) : Attribute
{
    public IEnumerable<string> InjectedArguments => injectedArguments;
}