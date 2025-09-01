using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ServerBound;

[PublicAPI]
[Packet("hello",false,"Login")]
public partial class LoginStart : IPacket
{
    [PacketField] public string Username;
    [PacketField] public Uuid PlayerUuid;
}