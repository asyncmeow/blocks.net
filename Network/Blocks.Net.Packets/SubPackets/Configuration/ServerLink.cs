using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.SubPackets.Configuration;

[PublicAPI]
[SubPacket]
public partial struct ServerLink
{
    // Can either be an integer or a text component
    [PacketField] public Primitives.Nbt Label;
    [PacketField] public string Url;
}