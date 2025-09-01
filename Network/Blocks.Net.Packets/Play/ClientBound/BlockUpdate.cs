using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("block_update", true, "Play")]
public partial class BlockUpdate : IPacket
{
    [PacketField] public Position Location;
    [PacketField] public BlockState State;
}