using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Status.ServerBound;

[PublicAPI]
[Packet(0x00,false,"Status")]
public partial class StatusRequest : IPacket;