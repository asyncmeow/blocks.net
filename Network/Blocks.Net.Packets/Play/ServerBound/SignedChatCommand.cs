using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("chat_command_signed",false,"Play")]
public partial class SignedChatCommand : IPacket
{
    [PacketField] public string Command;
    [PacketField] public long Timestamp;
    [PacketField] public long Salt;
    [PacketField] public ArgumentSignature[] ArgumentSignatures;
    [PacketField] public VarInt MessageCount;
    [PacketField] public FixedBitSet20 Acknowledged;
    [PacketField] public byte CheckSum;
}