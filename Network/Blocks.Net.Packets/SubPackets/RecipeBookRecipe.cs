using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct RecipeBookRecipe
{
    [PacketField] public VarInt RecipeId;
    [PacketField] public RecipeDisplayImpl Display;
    [PacketField] public VarInt GroupId;
    [PacketField] public RegistryReference CategoryId;
    [PacketField] public IdSet[]? Ingredients;
    [PacketField] public byte Flags;
}