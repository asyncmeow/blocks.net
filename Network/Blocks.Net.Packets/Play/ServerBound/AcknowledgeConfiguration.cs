using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("configuration_acknowledged",false,"Play")]
public partial class AcknowledgeConfiguration : IPacket;