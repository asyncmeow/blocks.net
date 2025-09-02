using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("waypoint",true,"Play")]
public partial class Waypoint : IPacket
{
    public enum Operations
    {
        Track,
        Untrack,
        Update
    }
    [PacketEnum(typeof(VarInt))] public Operations Operation;
    [PacketField] public UuidOrString Id;
    [PacketField] public Identifier IconStyle;
    [PacketField] public Byte3? Color;
    [PacketField] public WaypointDataImpl Data;
}