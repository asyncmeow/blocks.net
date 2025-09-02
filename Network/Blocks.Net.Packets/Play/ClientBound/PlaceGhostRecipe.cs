using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("place_ghost_recipe", true, "Play")]
public partial class PlaceGhostRecipe : IPacket
{
    [PacketField] public VarInt WindowId;
    [PacketField] public RecipeDisplayImpl Recipe;
}