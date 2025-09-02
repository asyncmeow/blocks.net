using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("recipe_book_settings",true,"Play")]
public partial class RecipeBookSettings : IPacket
{
    [PacketField] public bool CraftingRecipeBookOpen;
    [PacketField] public bool CraftingRecipeBookFilterActive;
    [PacketField] public bool SmeltingRecipeBookOpen;
    [PacketField] public bool SmeltingRecipeBookFilterActive;
    [PacketField] public bool BlastFurnaceRecipeBookOpen;
    [PacketField] public bool BlastFurnaceRecipeBookFilterActive;
    [PacketField] public bool SmokerRecipeBookOpen;
    [PacketField] public bool SmokerRecipeBookFilterActive;
}