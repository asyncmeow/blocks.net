using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("recipe_book_remove",true,"Play")]
public partial class RecipeBookRemove : IPacket
{
    [PacketField] public VarInt[] Recipes;
}