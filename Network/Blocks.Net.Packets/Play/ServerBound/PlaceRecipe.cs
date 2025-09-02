using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("place_recipe",false,"Play")]
public partial class PlaceRecipe : IPacket
{
    [PacketField] public VarInt WindowId;
    [PacketField] public VarInt RecipeId;
    [PacketField] public bool MakeAll;
}