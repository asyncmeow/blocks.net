using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x02,true,"Play")]
public partial class SpawnExperienceOrb : IPacket
{
    [PacketField] public VarInt Id;
    [PacketField] public double X;
    [PacketField] public double Y;
    [PacketField] public double Z;
    [PacketField] public short Count;
}