using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Enums;

[FieldedEnum(typeof(VarInt))]
public enum WaypointData
{
    Empty,
    Vec3I,
    Chunk,
    Azimuth
}

[EnumField(typeof(WaypointData))]
public partial class WaypointDataVec3I
{
    [PacketField] public VarInt X;
    [PacketField] public VarInt Y;
    [PacketField] public VarInt Z;
}

[EnumField(typeof(WaypointData))]
public partial class WaypointDataChunk
{
    [PacketField] public VarInt X;
    [PacketField] public VarInt Z;
}

[EnumField(typeof(WaypointData))]
public partial class WaypointDataAzimuth
{
    [PacketField] public float Angle;
}