using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x3D, true, "Play")]
public partial class LookAt : IPacket
{
    public enum LookAtFrom
    {
        Feet = 0,
        Eyes = 1
    }

    [PacketEnum(typeof(VarInt))] public LookAtFrom From;
    [PacketField] public double TargetX;
    [PacketField] public double TargetY;
    [PacketField] public double TargetZ;
    [PacketField] public bool IsEntity;
    [PacketOptionalField("IsEntity")] public VarInt EntityId;
    [PacketOptionalField("IsEntity")] public VarInt EntityFeetEyes;
    public LookAtFrom EntityTarget => (LookAtFrom)EntityFeetEyes.Value;

}