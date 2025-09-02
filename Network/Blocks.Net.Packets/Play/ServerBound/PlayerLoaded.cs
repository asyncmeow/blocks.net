using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("player_loaded",false,"Play")]
public partial class PlayerLoaded : IPacket;