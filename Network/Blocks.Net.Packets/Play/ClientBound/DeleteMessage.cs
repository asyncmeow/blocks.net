using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("delete_chat", true, "Play")]
public partial class DeleteMessage : IPacket
{
    [PacketField] public VarInt MessageId;
    [PacketField] public bool HasSignature = true;
    [PacketOptionalArrayField(nameof(HasSignature),"256")] public byte[] Signature;
}