using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("custom_payload",true,"Play")]
public partial class ClientboundPluginMessage : IPacket
{
    [PacketField] public Identifier Channel;
    [PacketField] public LengthInferredByteArray Data;
}