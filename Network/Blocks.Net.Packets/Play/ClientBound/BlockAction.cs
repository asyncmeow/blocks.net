using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x08,true,"Client")]
public partial class BlockAction : IPacket
{
    [PacketField] public Position Location;
    [PacketField] public byte Id;
    [PacketField] public byte Parameter;
    [PacketField] public VarInt BlockType;
}