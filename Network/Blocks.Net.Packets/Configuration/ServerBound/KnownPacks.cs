using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.Configuration;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ServerBound;

[PublicAPI]
[Packet("select_known_packs", false, "Configuration")]
public partial class ServerBoundKnownPacks : IPacket
{
    [PacketField] public KnownPacks[] Packs;
}