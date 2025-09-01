using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("block_event",true,"Play")]
public partial class BlockAction : IPacket
{
    [PacketField] public Position Location;
    [PacketField] public byte ActionId;
    [PacketField] public byte ActionParameter;
    [PacketField] public RegistryReference BlockType = new(0);
}