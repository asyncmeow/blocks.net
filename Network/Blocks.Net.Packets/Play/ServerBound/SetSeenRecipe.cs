using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("recipe_book_seen_recipe",false,"Play")]
public partial class SetSeenRecipe : IPacket
{
    [PacketField] public VarInt RecipeId;
}