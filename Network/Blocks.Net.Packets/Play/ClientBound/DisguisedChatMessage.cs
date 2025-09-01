using Blocks.Net.Nbt;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("disguised_chat", true, "Play")]
public partial class DisguisedChatMessage : IPacket
{
    [PacketField] public NbtTag Message;
    [PacketField] public IdOrChatType ChatType;
    [PacketField] public NbtTag SenderName;
    [PacketField] public NbtTag? TargetName;
}