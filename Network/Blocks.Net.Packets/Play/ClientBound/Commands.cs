using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x11,true,"Play")]
public partial class Commands : IPacket
{
    [PacketField] public VarInt Count;
    [PacketArrayField("Count")] public CommandData[] Nodes;
    [PacketField] public VarInt RootIndex;
}