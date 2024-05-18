using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x18,true,"Play")]
public partial class PluginMessage : IPacket
{
    [PacketField] public Identifier Channel;
    [PacketField] public LengthInferredByteArray Data;
}