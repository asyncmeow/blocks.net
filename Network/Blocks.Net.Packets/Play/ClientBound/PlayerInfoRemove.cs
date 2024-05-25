using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x3B,true,"Play")]
public partial class PlayerInfoRemove : IPacket
{
    [PacketField] public VarInt NumberOfPlayers;
    [PacketArrayField("NumberOfPlayers")] public Guid[] Players;
}