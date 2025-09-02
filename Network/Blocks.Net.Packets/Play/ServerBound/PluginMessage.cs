using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("custom_payload",false,"Play")]
public partial class PluginMessage : IPacket
{
    [PacketField] public Identifier Channel;
    [PacketField] public LengthInferredByteArray Data;
}