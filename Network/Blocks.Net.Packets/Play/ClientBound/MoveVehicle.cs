using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("move_vehicle", true, "Play")]
public partial class MoveVehicle : IPacket
{
    [PacketField] public Double3 Position;
    [PacketField] public float Yaw;
    [PacketField] public float Pitch;
}