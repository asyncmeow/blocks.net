using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ServerBound;

[PublicAPI]
[Packet("login_acknowledged",false,"Login")]
public partial class LoginAcknowledged : IPacket;