using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ServerBound;

[PublicAPI]
[Packet("finish_configuration",false,"Configuration")]
public partial class AcknowledgeFinishConfiguration : IPacket;