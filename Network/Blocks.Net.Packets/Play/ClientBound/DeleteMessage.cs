using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x1A,true,"Play")]
public partial class DeleteMessage : IPacket
{
    [PacketField] public VarInt MessageId;
    [PacketArrayField("256")] public byte[] Signature;
}