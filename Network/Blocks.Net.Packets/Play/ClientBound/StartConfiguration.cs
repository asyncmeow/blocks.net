using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;


[Packet("start_configuration",true,"Play")]
public partial class StartConfiguration : IPacket;