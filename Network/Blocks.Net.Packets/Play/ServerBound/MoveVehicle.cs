using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("move_vehicle",false,"Play")]
public partial class MoveVehicle : IPacket
{
    [PacketField] public Double3 Position;
    [PacketField] public float Yaw;
    [PacketField] public float Pitch;
    [PacketField] public bool OnGround; // TODO: Confirm this ones existence
}