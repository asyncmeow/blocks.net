using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x24,true,"Play")]
public partial class InitializeWorldBorder : IPacket
{
    [PacketField] public double X;
    [PacketField] public double Z;
    [PacketField] public double OldDiameter;
    [PacketField] public double NewDiameter;
    [PacketField] public VarLong Speed;
    [PacketField] public VarInt PortalTeleportBoundary;
    [PacketField] public VarInt WarningBlocks;
    [PacketField] public VarInt WarningTime;
}