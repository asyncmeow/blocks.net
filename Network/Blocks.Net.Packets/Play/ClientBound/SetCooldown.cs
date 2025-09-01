using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("cooldown",true,"Play")]
public partial class SetCooldown : IPacket
{
    [PacketField] public Identifier CooldownGroup;
    [PacketField] public VarInt CooldownTicks;
}