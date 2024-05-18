
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x20,true,"Play")]
public partial class GameEvent : IPacket
{
    [PacketField] public byte Event;
    [PacketField] public float Value;
}