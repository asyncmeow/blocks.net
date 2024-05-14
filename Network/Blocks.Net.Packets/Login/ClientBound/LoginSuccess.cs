using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ClientBound;

[PublicAPI]
[Packet(0x02, true, "Login")]
public partial class LoginSuccess : IPacket
{
    [PacketField] public Uuid PlayerUuid;
    [PacketField] public string Username;
    [PacketField] public VarInt NumProperties;
    [PacketArrayField("NumProperties")] public PlayerProperty[] PlayerProperties;
}