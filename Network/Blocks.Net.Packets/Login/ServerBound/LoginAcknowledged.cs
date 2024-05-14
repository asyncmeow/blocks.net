using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ServerBound;

[PublicAPI]
[Packet(0x03,false,"Login")]
public partial class LoginAcknowledged : IPacket;