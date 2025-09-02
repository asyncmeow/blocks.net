using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("chat_session_update",false,"Play")]
public partial class PlayerSession : IPacket
{
    [PacketField] public Uuid SessionId;
    [PacketField] public PublicKey Key;
}