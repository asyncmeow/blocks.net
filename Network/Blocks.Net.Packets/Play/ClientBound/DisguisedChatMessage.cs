using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x1C,true,"Play")]
public partial class DisguisedChatMessage : IPacket
{
    // TODO: Make TextComponent and JsonTextComponent primitives, and conversions to and from them
    [PacketField] public NbtTag Message;
    [PacketField] public VarInt ChatType;
    [PacketField] public NbtTag SenderName;
    [PacketField] public bool HasTargetName;
    [PacketOptionalField("HasTargetName")] public NbtTag TargetName;
}