using Blocks.Net.DataTypes;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Primitives;

[SubPacket]
public partial struct IdSet
{
    [PacketField] public VarInt Type;
    /// <summary>
    /// The registry tag defining the ID set. Only present if Type is 0. 
    /// </summary>
    [PacketOptionalField("Type == 0")] public Identifier Identifier;

    /// <summary>
    /// An array of registry IDs. Only present if Type is not 0.
    /// The size of the array is equal to Type - 1. 
    /// </summary>
    [PacketOptionalArrayField("Type != 0", "Type - 1")]
    public RegistryReference[] Identifiers;
}