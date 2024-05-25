using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x35,true,"Play")]
public partial class PlaceGhostRecipe : IPacket
{
    [PacketField] public byte WindowId;
    [PacketField] public Identifier Recipe;
}