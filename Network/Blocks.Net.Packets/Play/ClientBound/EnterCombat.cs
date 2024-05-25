using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x39,true,"Play")]
public partial class EnterCombat : IPacket;