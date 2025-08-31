using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x10, true, "Play")]
public partial class Commands : IPacket
{
    [PacketField] public CommandData[] Nodes;
    [PacketField] public VarInt RootIndex;
}