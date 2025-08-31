using Blocks.Net.Nbt;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.SubPackets.Configuration;

[PublicAPI]
[SubPacket]
public partial struct ServerLink
{
    // Can either be an integer or a text component
    [PacketField] public NbtTag? Label;
    [PacketField] public string Url;
}