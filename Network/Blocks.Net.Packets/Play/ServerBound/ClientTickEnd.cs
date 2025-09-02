using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("client_tick_end",false,"Play")]
public partial class ClientTickEnd : IPacket;