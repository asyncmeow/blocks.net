using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x3A,true,"Play")]
public partial class CombatDeath : IPacket
{
    [PacketField] public VarInt PlayerId;
    [PacketField] public NbtTag Message;
}