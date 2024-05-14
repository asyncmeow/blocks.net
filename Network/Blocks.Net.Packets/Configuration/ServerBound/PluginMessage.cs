using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ServerBound;

[PublicAPI]
[Packet(0x01,false,"Configuration")]
public partial class PluginMessage : IPacket
{
    [PacketField] public string Channel;
    [PacketField] public LengthInferredByteArray Data;
}