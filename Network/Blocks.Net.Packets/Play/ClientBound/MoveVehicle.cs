using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x2E,true,"Play")]
public partial class MoveVehicle : IPacket
{
    [PacketField] public double X;
    [PacketField] public double Y;
    [PacketField] public double Z;
    [PacketField] public float Yaw;
    [PacketField] public float Pitch;
}