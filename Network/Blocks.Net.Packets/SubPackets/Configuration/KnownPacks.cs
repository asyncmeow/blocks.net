using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.SubPackets.Configuration;

[PublicAPI]
[SubPacket]
public partial struct KnownPacks
{
    [PacketField] public string Namespace;
    [PacketField] public string Id;
    [PacketField] public string Version;
}