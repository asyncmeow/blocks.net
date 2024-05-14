using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ServerBound;

[PublicAPI]
[Packet(0x02,false,"Configuration")]
public partial class AcknowledgeFinishConfiguration : IPacket;