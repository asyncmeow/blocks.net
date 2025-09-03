using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("player_combat_enter",true,"Play")]
public partial class EnterCombat : IPacket;