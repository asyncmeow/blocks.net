using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Status.ServerBound;

[PublicAPI]
[Packet("status_request",false,"Status")]
public partial class StatusRequest : IPacket;