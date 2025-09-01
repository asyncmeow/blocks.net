using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ClientBound;

[PublicAPI]
[Packet("login_finished",true, "Login")]
public partial class LoginSuccess : IPacket
{
    [PacketField] public Uuid PlayerUuid;
    [PacketField] public string Username;
    [PacketField] public PlayerProperty[] PlayerProperties;
}