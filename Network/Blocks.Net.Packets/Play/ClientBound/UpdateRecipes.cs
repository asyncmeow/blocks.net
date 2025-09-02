using Blocks.Net.Nbt.Utilities;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("update_recipes",true,"Play")]
public partial class UpdateRecipes : IPacket
{
    [PacketField] public PropertySet[] PropertySets;
    [PacketField] public StonecutterRecipe[] StonecutterRecipes;
}