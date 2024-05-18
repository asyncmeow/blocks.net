using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x19,true,"Play")]
public partial class DamageEvent : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public VarInt SourceTypeId;
    
    /// <summary>
    /// The ID + 1 of the entity responsible for the damage, if present. If not present, the value is 0
    /// </summary>
    [PacketField] public VarInt SourceCauseId;

    /// <summary>
    /// <para>
    /// The ID + 1 of the entity that directly dealt the damage, if present. If not present, the value is 0. If this field is present:</para>
    /// <para>    - and damage was dealt indirectly, such as by the use of a projectile, this field will contain the ID of such projectile;</para>
    /// <para>    - and damage was dealt directly, such as by manually attacking, this field will contain the same value as Source Cause ID.</para>
    /// </summary>
    [PacketField] public VarInt SourceDirectId;

    [PacketField] public bool HasSourcePosition;

    [PacketOptionalField("HasSourcePosition")]
    public double SourcePositionX;

    [PacketOptionalField("HasSourcePosition")]
    public double SourcePositionY;

    [PacketOptionalField("HasSourcePosition")]
    public double SourcePositionZ;
}