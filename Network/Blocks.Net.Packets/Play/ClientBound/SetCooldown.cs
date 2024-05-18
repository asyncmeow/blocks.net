using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x16,true,"Play")]
public partial class SetCooldown : IPacket
{
    [PacketField] public VarInt ItemId;
    [PacketField] public VarInt CooldownTicks;
}