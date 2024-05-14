using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet(0x00,true,"Configuration")]
public partial class PluginMessage : IPacket
{
    [PacketField] public string Channel;
    [PacketField] public LengthInferredByteArray Data;
}