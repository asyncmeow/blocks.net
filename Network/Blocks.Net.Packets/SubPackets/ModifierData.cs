using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct ModifierData
{
    [PacketField] public Identifier Id;
    [PacketField] public double Amount;
    [PacketField] public byte Operation;
}