using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x3E,true,"Play")]
public partial class SynchronizePlayerPosition : IPacket
{
    [Flags]
    public enum SynchronizeFlags
    {
        RelativeX = 0x01,
        RelativeY = 0x02,
        RelativeZ = 0x04,
        RelativePitch = 0x08,
        RelativeYaw = 0x10
    }
    
    [PacketField] public double X;
    [PacketField] public double Y;
    [PacketField] public double Z;
    [PacketField] public float Yaw;
    [PacketField] public float Pitch;
    [PacketField] public float Roll;
    [PacketEnum(typeof(byte))] public SynchronizeFlags Flags;
    [PacketField] public VarInt TeleportId;
}