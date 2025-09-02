using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("recipe_book_add",true,"Play")]
public partial class RecipeBookAdd : IPacket
{
    [PacketField] public RecipeBookRecipe[] Recipes;
    [PacketField] public bool Replace;
}