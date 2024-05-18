using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x26,true,"Play")]
public partial class WorldEvent : IPacket
{
    [PacketField] public int Event;
    [PacketField] public Position Location;
    [PacketField] public int Data;
    [PacketField] public bool DisableRelativeVolume;
}